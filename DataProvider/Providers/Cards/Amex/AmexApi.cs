using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataProvider.Providers.Cards.Amex.Dto;
using DataProvider.Providers.Exceptions;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataProvider.Providers.Cards.Amex
{
    public class AmexApi : HttpScrapper, IAmexApi
    {
        #region Constants
        private const string Name = "Amex";
        private const string LoginDomain = "https://he.americanexpress.co.il";

        private const string AspDotNetSessionId = "ASP.NET_SessionId";
        private const string Jsessionid = "JSESSIONID";
        private const string Alt50ZLinuxPrd = "Alt50_ZLinuxPrd";
        private const string ServiceP = "ServiceP";
        private const string RequestVerificationToken = "__RequestVerificationToken";

        private readonly string _idNumber;
        #endregion

        private readonly AmexSessionInfo _sessionInfo;

        public AmexApi(IDictionary<string, string> credentials)
        {
            if (credentials == null || credentials.Count != 3)
            {
                throw new ArgumentException("Credentials for access to Cal are incorrect.");
            }

            _sessionInfo = new AmexSessionInfo();

            var credentialValues = credentials.Values.ToArray();
            _idNumber = credentialValues[0];
            var lastDigits = credentialValues[1];
            var password = credentialValues[2];
            
            try
            {
                _sessionInfo.AntiForgeryToken = GetVerificationToken();
                ValidateId(_idNumber, lastDigits);
                Login(_idNumber, lastDigits, password);
            }
            catch (Exception exp)
            {
                throw new LoginException(Name, _idNumber) { Error = exp.Message };
            }
        }

        public IEnumerable<AmexCardInfo> GetCards()
        {
            var api = "/services/ProxyRequestHandler.ashx?reqName=CardsList_102Digital";
            var baseAddress = new Uri(LoginDomain);

            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseAddress, new Cookie(Jsessionid, _sessionInfo.Jsessionid));
            cookieContainer.Add(baseAddress, new Cookie(Alt50ZLinuxPrd, _sessionInfo.Alt50_ZLinuxPrd));
            cookieContainer.Add(baseAddress, new Cookie(ServiceP, _sessionInfo.ServiceP));
            cookieContainer.Add(baseAddress, new Cookie(RequestVerificationToken, _sessionInfo.RequestVerificationToken));
            cookieContainer.Add(baseAddress, new Cookie(AspDotNetSessionId, _sessionInfo.AspDotNetSessionId));

            var headers = new List<Tuple<string, string>>
            {
                new Tuple<string, string> (RequestVerificationToken,  _sessionInfo.AntiForgeryToken),
            };

            var response = CallGetRequest<AmexCardListResponse>(baseAddress, api, cookieContainer, headers);

            if (IsError(response.Header))
            {
                throw new Exception($"Error status returned from get cards call: {GetError(response.Header)}");
            }

            return FetchValidCards(response);
        }
        
        public IEnumerable<AmexCardTransaction> GetTransactions(int cardIndex, int year, int month)
        {
            var arguments = $"&month={month:00}&year={year:0000}&cardIdx={cardIndex}";
            var api = "/services/ProxyRequestHandler.ashx?reqName=CardsTransactionsList" + arguments;
            var baseAddress = new Uri(LoginDomain);

            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseAddress, new Cookie(Jsessionid, _sessionInfo.Jsessionid));
            cookieContainer.Add(baseAddress, new Cookie(Alt50ZLinuxPrd, _sessionInfo.Alt50_ZLinuxPrd));
            cookieContainer.Add(baseAddress, new Cookie(ServiceP, _sessionInfo.ServiceP));
            cookieContainer.Add(baseAddress, new Cookie(RequestVerificationToken, _sessionInfo.RequestVerificationToken));
            cookieContainer.Add(baseAddress, new Cookie(AspDotNetSessionId, _sessionInfo.AspDotNetSessionId));

            var headers = new List<Tuple<string, string>>
            {
                new Tuple<string, string> (RequestVerificationToken,  _sessionInfo.AntiForgeryToken),
            };

            var response = CallGetRequest(baseAddress, api, cookieContainer, headers);
            var result = new AmexExpensesInfo();
            try
            {
                result = FetchExpensesInfo(response);
            }
            catch (Exception exp)
            {
                throw new Exception($"Exit in {Name} api failed, for user: {_idNumber} with error: {exp.Message}");
            }
            
            return result.Transactions;
        }

        public IEnumerable<CardChargeResponse> GetBankDebit(string accountNumber, String cardNumber, int year, int month)
        {
            var arguments = $"&format=Json&actionCode=3" +
                            $"&accountNumber={accountNumber}" +
                            $"&billingDate={year:0000}-{month:00}-01";
            var api = "/services/ProxyRequestHandler.ashx?reqName=DashboardMonth" + arguments;
            var baseAddress = new Uri(LoginDomain);

            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseAddress, new Cookie(Jsessionid, _sessionInfo.Jsessionid));
            cookieContainer.Add(baseAddress, new Cookie(Alt50ZLinuxPrd, _sessionInfo.Alt50_ZLinuxPrd));
            cookieContainer.Add(baseAddress, new Cookie(ServiceP, _sessionInfo.ServiceP));
            cookieContainer.Add(baseAddress, new Cookie(RequestVerificationToken, _sessionInfo.RequestVerificationToken));
            cookieContainer.Add(baseAddress, new Cookie(AspDotNetSessionId, _sessionInfo.AspDotNetSessionId));

            var headers = new List<Tuple<string, string>>
            {
                new Tuple<string, string> (RequestVerificationToken,  _sessionInfo.AntiForgeryToken),
            };

            var response = CallGetRequest<DashboardMonthResponse>(baseAddress, api, cookieContainer, headers);
            if (response == null || IsError(response.Header))
            {
                var error = GetError(response?.Header);
                throw new Exception($"Exit in {Name} api failed, for user: {_idNumber}, with error: {error}");
            }

            return response.DashboardMonthBean.CardsCharges.Where(cc => cc.CardNumber == cardNumber);
        }

        public DealDetails GetTransactionDetails(int cardIndex, string paymentDate, Boolean isInbound, string transactionId)
        {
            var inState = isInbound ? "yes" : "no";
            var arguments = $"&CardIndex={cardIndex}&moedChiuv={paymentDate}&inState={inState}&shovarRatz={transactionId}";
            var api = "/services/ProxyRequestHandler.ashx?reqName=PirteyIska_204" + arguments;
            var baseAddress = new Uri(LoginDomain);

            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseAddress, new Cookie(Jsessionid, _sessionInfo.Jsessionid));
            cookieContainer.Add(baseAddress, new Cookie(Alt50ZLinuxPrd, _sessionInfo.Alt50_ZLinuxPrd));
            cookieContainer.Add(baseAddress, new Cookie(ServiceP, _sessionInfo.ServiceP));
            cookieContainer.Add(baseAddress, new Cookie(RequestVerificationToken, _sessionInfo.RequestVerificationToken));
            cookieContainer.Add(baseAddress, new Cookie(AspDotNetSessionId, _sessionInfo.AspDotNetSessionId));

            var response = CallGetRequest<TransactionDetailsResponse>(baseAddress, api, cookieContainer, null);
            if (response == null || IsError(response.Header))
            {
                var error = GetError(response?.Header);
                throw new Exception($"Transaction details in {Name} api failed, for user: {_idNumber}, with error: {error}");
            }

            return response.PirteyIska_204Bean;
        }
        
        private String GetVerificationToken()
        {
            var api = "/personalarea/login/";

            var html = CallGetRequest(new Uri(LoginDomain), api, new CookieContainer(), null);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var t = doc.DocumentNode.Descendants("input").
                FirstOrDefault(a => a.Attributes.Contains("name") && a.Attributes["name"].Value.Equals(RequestVerificationToken));
            

            return t?.Attributes["value"].Value;
        }
        
        private String ValidateId(string idNumber, string lastDigits)
        {
            var api = "/services/ProxyRequestHandler.ashx?reqName=ValidateIdData";
            var baseAddress = new Uri(LoginDomain);

            var body = new ValidateIdRequestBody
            {
                Id = idNumber,
                CardSuffix = lastDigits,
                CheckLevel = "1",
                CompanyCode = "77",
                IdType = "1",
                CountryCode = "212"
            };

            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/x-www-form-urlencoded");

            var headers = new List<Tuple<string, string>>
            {
                new Tuple<String, String>(RequestVerificationToken, _sessionInfo.AntiForgeryToken)
            };

            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseAddress, new Cookie(RequestVerificationToken, _sessionInfo.RequestVerificationToken));
            cookieContainer.Add(baseAddress, new Cookie(AspDotNetSessionId, _sessionInfo.AspDotNetSessionId));

            var response = CallPostRequest<AmexValidateIdResponse>(baseAddress, api, content, cookieContainer, headers);
            if (response == null || IsError(response.Header))
            {
                var error = GetError(response?.Header);
                throw new LoginException(Name, idNumber) { Error = error };
            }

            return response.ValidateIdDataBean.UserName;
        }

        private AmexLoginResponse Login(string idNumber, string lastDigits, string password)
        {
            var api = "/services/ProxyRequestHandler.ashx?reqName=performLogonA";
            var baseAddress = new Uri(LoginDomain);
            var body = new AmexLogonBody
            {
                MisparZihuy = idNumber,
                CountryCode = "212",
                IdType = "1",
                Sisma = password,
                CardSuffix = lastDigits
            };

            var headers = new List<Tuple<string, string>>
            {
                new Tuple<string, string> (RequestVerificationToken,  _sessionInfo.AntiForgeryToken),
            };

            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseAddress, new Cookie(RequestVerificationToken, _sessionInfo.RequestVerificationToken));

            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = CallPostRequest<AmexLoginResponse>(baseAddress, api, content, cookieContainer, headers);
            if (response.Status != 1)
            {
                throw new LoginException(Name, idNumber) { Error = response.Message };
            }

            return response;
        }

        private static bool IsError(AmexHeaderResponse header)
        {
            return header == null || header.Status != 1;
        }

        private static string GetError(AmexHeaderResponse header)
        {
            if (header?.Status == 0)
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(header?.Message))
            {
                return "Unknown error";
            }

            return header.Message;
        }

        private static IEnumerable<AmexCardInfo> FetchValidCards(AmexCardListResponse response)
        {
            var cards = new List<AmexCardInfo>();
            if (response.CardsList_102DigitalBean.Table1 != null)
            {
                cards.AddRange(response.CardsList_102DigitalBean.Table1);
            }
            if (response.CardsList_102DigitalBean.Table2 != null)
            {
                cards.AddRange(response.CardsList_102DigitalBean.Table2);
            }

            var result = cards.Where(card =>
            {
                var splitedDates = card.StatusDate.Split('.');
                return splitedDates[2] == "0001";
            });
            return result;
        }

        private static AmexExpensesInfo FetchExpensesInfo(string response)
        {
            string result = response;
            JObject json = JObject.Parse(result);

            var cardsTransactionsListProperty = json.Property("CardsTransactionsListBean");
            if (cardsTransactionsListProperty == null)
            {
                throw new Exception($"Cannot to get transactions. Error: CardsTransactionsListBean is missing");
            }

            var cardsTransactionsList = cardsTransactionsListProperty.Value as JObject;

            result = FormatTransactionsList(cardsTransactionsList, result, @"^card[0-9]*$", "card");
            result = FormatTransactionsList(cardsTransactionsList, result, @"^index[0-9]*$", "index");
            result = FormatTransactionsList(cardsTransactionsList, result, @"^id[0-9]+$", "id");

            var data = JsonConvert.DeserializeObject<AmexTransactionsListResponse>(result);
            if (IsError(data.Header))
            {
                throw new Exception($"Cannot to get transactions. Error: {GetError(data.Header)}");
            }
            var inboundTransactions = data.CardsTransactionsListBean.Index.CurrentCardTransactions.First().TxnIsrael.ToList();
            var outboundTransactions = data.CardsTransactionsListBean.Index.CurrentCardTransactions.Last().TxnAbroad.ToList();
            var transactions = inboundTransactions.Concat(outboundTransactions);

            var charge = data.CardsTransactionsListBean.TotalChargeNis;

            return new AmexExpensesInfo
            {
                Transactions = transactions  ?? new List<Dto.AmexCardTransaction>(),
                NextCharge = Convert.ToDecimal(charge)
            };
        }

        private static string FormatTransactionsList(JObject cardsTransactionsList, string json, string pattern, string newText)
        {
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            List<JProperty> keys = cardsTransactionsList.Properties().Where(p =>
            {
                Match match = regex.Match(p.Name);
                return match.Success;
            }).ToList();

            string result = string.Empty;
            foreach (var key in keys)
            {
                result = json.Replace(key.Name, newText);
            }

            return result;
        }


        protected override void ExtractCookies(HttpResponseMessage response)
        {
            IEnumerable<string> cookies = new List<string>();
            try
            {
                if (response.Headers != null && response.Headers.Contains("set-cookie"))
                {
                    cookies = response.Headers.GetValues("set-cookie");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            if (cookies == null)
            {
                return;
            }

            foreach (var setValue in cookies)
            {
                var items = setValue.Split(';');
                foreach (var item in items)
                {
                    var nameValue = item.Split('=');

                    if (nameValue[0].Equals(Jsessionid))
                    {
                        _sessionInfo.Jsessionid = nameValue[1];
                    }
                    else if (nameValue[0].Equals(Alt50ZLinuxPrd))
                    {
                        _sessionInfo.Alt50_ZLinuxPrd = nameValue[1];
                    }
                    else if (nameValue[0].Equals(ServiceP))
                    {
                        _sessionInfo.ServiceP = nameValue[1];
                    }
                    else if (nameValue[0].Equals(RequestVerificationToken))
                    {
                        _sessionInfo.RequestVerificationToken = nameValue[1];
                    }
                    else if (nameValue[0].Equals(AspDotNetSessionId))
                    {
                        _sessionInfo.AspDotNetSessionId = nameValue[1];
                    }
                }
            }
        }

        protected override void Exit()
        {
            var api = "/services/ProxyRequestHandler.ashx?reqName=performExit";
            var baseAddress = new Uri(LoginDomain);

            var headers = new List<Tuple<string, string>>
            {
                new Tuple<String, String>(RequestVerificationToken, _sessionInfo.AntiForgeryToken)
            };
            
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseAddress, new Cookie(Jsessionid, _sessionInfo.Jsessionid));
            cookieContainer.Add(baseAddress, new Cookie(RequestVerificationToken, _sessionInfo.RequestVerificationToken));
            cookieContainer.Add(baseAddress, new Cookie(AspDotNetSessionId, _sessionInfo.AspDotNetSessionId));

            try
            {
                CallPostRequest<object>(baseAddress, api, null, cookieContainer, headers);
            }
            catch (Exception exp)
            {
                throw new Exception($"Exit in {Name} api failed, for user: {_idNumber}, with error: {exp.Message}");
            }
        }

        protected override void ExitErrorHandler(Task task, object context)
        {
            throw new Exception($"Exit in {Name} api failed, for user: {_idNumber}, with error: {task?.Exception?.Message}");
        }
    }
}