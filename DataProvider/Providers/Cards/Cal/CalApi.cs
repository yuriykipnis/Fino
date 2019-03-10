using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using DataProvider.Providers.Cards.Amex;
using DataProvider.Providers.Cards.Cal.Dto;
using DataProvider.Providers.Exceptions;
using GoldMountainShared.Storage.Documents;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace DataProvider.Providers.Cards.Cal
{
    public class CalApi : HttpScrapper, ICalApi
    {
        #region Constants
        private const string Name= "Visa Cal";

        private const string LoginDomain = "https://connect.cal-online.co.il";
        private const string ServicesDomain = "https://services.cal-online.co.il";
        private const string Cal4UDomain = "https://cal4u.cal-online.co.il";

        private const string WebClientId = "4057F41C-BABB-416A-87F4-7DF1FC25DB2E";
        private const string AndroidClientId = "05D905EB-810A-4680-9B23-1A2AC46533BF";
        private readonly string _username;
        #endregion

        private readonly CalSessionInfo _sessionInfo;

        public CalApi(IDictionary<string, string> credentials)
        {
            if (credentials == null || !credentials.Any() || credentials.Count != 2)
            {
                throw new ArgumentException("Credentials for access to Cal are incorrect.");
            }

            var credentialValues = credentials.Values.ToArray();
            var username = credentialValues[0];
            var password = credentialValues[1];

            _sessionInfo = new CalSessionInfo();
            _username = username;

            try
            {
                var token = Login(username, password);
                _sessionInfo.Token = token;
            }
            catch (Exception exp)
            {
                throw new LoginException(Name, username) { Error = exp.Message };
            }
        }

        private string Login(string username, string password)
        {
            var api = "/api/authentication/login";
            string body = $"{{\"username\":\"{username}\"," +
                          $"\"password\":\"{password}\"," +
                          $"\"rememberMe\":null}}";

            var headers = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("X-Site-Id", AndroidClientId),
            };

            var response = CallPostRequest<CalLoginResponse>(new Uri(LoginDomain), api,
                new StringContent(body, Encoding.UTF8, "application/json"),
                null, headers);
            return response.Token;
        }
        
        public CalGetCardsResponse GetCards()
        {
            var api = "/Cal4U/CardsByAccounts";

            var headers = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("X-Site-Id", AndroidClientId),
                new Tuple<string, string>("Authorization", $"CALAuthScheme {_sessionInfo.Token}" )
            };

            var response = CallGetRequest<CalGetCardsResponse>(new Uri(Cal4UDomain), api, null, headers);
            if (!response.Response.Status.Succeeded)
            {
                throw new Exception($"Did not succeed to fetch cards for user {_username}. Error: {response.Response.Status.Message}");
            }

            foreach (var account in response.BankAccounts)
            {
                account.Cards = account.Cards.Where(c => String.IsNullOrEmpty(c.CardStatus));
            }
            
            return response;
        }

        public IEnumerable<CalBankDebit> GetBankDebits(string bankAccountId, string cardId, DateTime startDate, DateTime endDate)
        {
            var api = $"/Cal4U/CalBankDebits/{bankAccountId}?" +
                      $"DebitLevel=A&DebitType=2" +
                      $"&FromMonth={startDate.Month}&FromYear={startDate.Year}" +
                      $"&ToMonth={endDate.Month}&ToYear={endDate.Year}" +
                      $"&cardID={cardId}";

            var headers = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("X-Site-Id", AndroidClientId),
                new Tuple<string, string>("Authorization", $"CALAuthScheme {_sessionInfo.Token}" )
            };

            var response = CallGetRequest<CalBankDebitsResponse>(new Uri(Cal4UDomain), api, null, headers);
            if (!response.Response.Status.Succeeded)
            {
                throw new Exception($"Did not succeed to fetch debits for card {cardId}. Error: {response.Response.Status.Message}");
            }

            return response.Debits.Where(d => d.CardId.Equals(cardId)).ToList();
        }

        public IEnumerable<CalTransactionResponse> GetTransactions(string cardId, DateTime startDate, DateTime endDate)
        {
            var api = $"/Cal4U/CalTransactions/{cardId}" +
                      $"?FromDate={startDate:dd/MM/yyyy}&ToDate={endDate:dd/MM/yyyy}";

            var headers = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("X-Site-Id", AndroidClientId),
                new Tuple<string, string>("Authorization", $"CALAuthScheme {_sessionInfo.Token}")
            };

            var response = CallGetRequest<CalTransactionsResponse>(new Uri(Cal4UDomain), api, null, headers);
            if (!response.Response.Status.Succeeded)
            {
                throw new Exception($"Did not succeed to fetch transaction in card {cardId}. Error: {response.Response.Status.Message}");
            }

            return response.Transactions ?? new List<CalTransactionResponse>();
        }

        public CalTransactionDetailsResponse GetTransactionDetails(string transactionId, int numerator)
        {
            var api = $"/Cal4U/CalTransDetails/{transactionId}?Numerator={numerator}";

            var headers = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("X-Site-Id", AndroidClientId),
                new Tuple<string, string>("Authorization", $"CALAuthScheme {_sessionInfo.Token}" )
            };

            var response = CallGetRequest<CalTransactionDetailsResponse>(new Uri(Cal4UDomain), api, null, headers);
            if (!response.Response.Status.Succeeded)
            {
                throw new Exception($"Did not succeed to fetch transaction details for transaction {transactionId}. Error: {response.Response.Status.Message}" );
            }

            return response;
        }

        protected override void ExtractCookies(HttpResponseMessage response)
        {
        }

        protected override void Exit()
        {
            var api = "/Cal4U/CalAuthenticator";
            var headers = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("X-Site-Id", AndroidClientId),
                new Tuple<string, string>("Authorization", $"CALAuthScheme {_sessionInfo.Token}" )
            };

            try
            {
                CallDeleteRequest(new Uri(Cal4UDomain), api, null, headers);
            }
            catch (Exception exp)
            {
                throw new Exception($"Exit in {Name} api failed, for user: {_username}, with error: {exp.Message}");
            }
        }

        protected override void ExitErrorHandler(Task task, object context)
        {
            throw new Exception($"Exit in {Name} api failed, for user: {_username}, with error: {task?.Exception?.Message}");
        }
    }
}
