using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataProvider.Providers.Banks.Hapoalim.Dto;
using DataProvider.Providers.Exceptions;
using DataProvider.Providers.Models;
using GoldMountainShared.Storage.Documents;
using Newtonsoft.Json;
using Mortgage = DataProvider.Providers.Models.Mortgage;

namespace DataProvider.Providers.Banks.Hapoalim
{
    public class HapoalimApi : HttpScrapper, IHapoalimApi
    {
        #region Constants
        private const string DefaultOrganisation = "106402333";
        private const string LoginDomain = "https://login.bankhapoalim.co.il";
        private const string ActiveUser = "activeUser";
        private const string Smsession = "SMSESSION";
        private const string Jsessionid = "JSESSIONID";
        private const string Token = "token";
        private const string Lbinfologin = "lbinfologin";
        private const string SubDomain = "/ssb";
        //private const string SubDomain = "/ServerServices";
        #endregion

        #region Fields
        private HapoalimSessionInfo _sessionInfo;
        #endregion

        public HapoalimApi(IDictionary<string, string> credentials)
        {
            if (credentials == null || !credentials.Any() || credentials.Count != 2)
            {
                throw new ArgumentException("Credentials for access to Bank Hapoalim are incorrect.");
            }

            var crentialValues = credentials.Values.ToArray();
            var userId = crentialValues[0];
            var password = crentialValues[1];

            try
            {
                var initInfo = Init(userId);
                var result = Verify(userId, password);

                GetLandpage();
            }
            catch (Exception exp)
            {
                throw new LoginException("Bank Hapoalim", userId) { Error = exp.Message };
            }
        }

        public IEnumerable<HapoalimAccountResponse> GetAccounts()
        {
            var baseAddress = new Uri(LoginDomain);
            var cookies = new CookieContainer();
            cookies.Add(baseAddress, new Cookie(ActiveUser, _sessionInfo.ActiveUser));
            cookies.Add(baseAddress, new Cookie(Smsession, _sessionInfo.Smsession));
            cookies.Add(baseAddress, new Cookie(Jsessionid, _sessionInfo.Jsessionid));
            cookies.Add(baseAddress, new Cookie(Token, _sessionInfo.Token));
            cookies.Add(baseAddress, new Cookie(Lbinfologin, _sessionInfo.Lbinfologin));
            var api = SubDomain + "/general/accounts";
            
            var callResult = CallGetRequest(baseAddress, api, cookies, null);

            IList<HapoalimAccountResponse> result = null;
            result = JsonConvert.DeserializeObject<IList<HapoalimAccountResponse>>(callResult);
            return result;
        }

        public IEnumerable<HapoalimTransactionResponse> GetTransactions(BankAccount account, DateTime startTime, DateTime endTime)
        {
            var date1 = $"{startTime.Year}{startTime.Month:00}{startTime.Day:00}";
            var date2 = $"{endTime.Year}{endTime.Month:00}{endTime.Day:00}";

            var baseAddress = new Uri(LoginDomain);
            var cookies = new CookieContainer();
            cookies.Add(baseAddress, new Cookie(ActiveUser, _sessionInfo.ActiveUser));
            cookies.Add(baseAddress, new Cookie(Smsession, _sessionInfo.Smsession));
            cookies.Add(baseAddress, new Cookie(Jsessionid, _sessionInfo.Jsessionid));
            cookies.Add(baseAddress, new Cookie(Token, _sessionInfo.Token));
            cookies.Add(baseAddress, new Cookie(Lbinfologin, _sessionInfo.Lbinfologin));
            var api = SubDomain + $"/current-account/transactions?accountId=" +
                      $"{account.BankNumber}-{account.BranchNumber}-{account.AccountNumber}" +
                      $"&retrievalEndDate={date2}&retrievalStartDate={date1}";

            var accountsResponse = CallGetRequest(baseAddress, api, cookies, null);
            if (string.IsNullOrEmpty(accountsResponse)) 
            {
                return new List<HapoalimTransactionResponse>();
            }

            var content = JsonConvert.DeserializeObject<HapoalimTransactionsResponse>(accountsResponse);
            return content.Transactions;
        }
        
        public HapoalimMortgagesResponse GetMortgages(BankAccount account)
        {
            var baseAddress = new Uri(LoginDomain);
            var cookies = new CookieContainer();
            cookies.Add(baseAddress, new Cookie(ActiveUser, _sessionInfo.ActiveUser));
            cookies.Add(baseAddress, new Cookie(Smsession, _sessionInfo.Smsession));
            cookies.Add(baseAddress, new Cookie(Jsessionid, _sessionInfo.Jsessionid));
            cookies.Add(baseAddress, new Cookie(Token, _sessionInfo.Token));
            cookies.Add(baseAddress, new Cookie(Lbinfologin, _sessionInfo.Lbinfologin));
            var api = SubDomain + $@"/credit-and-mortgage/mortgages?accountId={account.BankNumber}-{account.BranchNumber}-{account.AccountNumber}";

            var accountsResponse = CallGetRequest(baseAddress, api, cookies, null);
            if (string.IsNullOrEmpty(accountsResponse))
            {
                return new HapoalimMortgagesResponse();
            }

            var content = JsonConvert.DeserializeObject<HapoalimMortgagesResponse>(accountsResponse);
            return content;
        }

        public HapoalimMortgageAssetResponse GetAssetForMortgage(BankAccount account, string loanId)
        {
            var baseAddress = new Uri(LoginDomain);
            var cookies = new CookieContainer();
            cookies.Add(baseAddress, new Cookie(ActiveUser, _sessionInfo.ActiveUser));
            cookies.Add(baseAddress, new Cookie(Smsession, _sessionInfo.Smsession));
            cookies.Add(baseAddress, new Cookie(Jsessionid, _sessionInfo.Jsessionid));
            cookies.Add(baseAddress, new Cookie(Token, _sessionInfo.Token));
            cookies.Add(baseAddress, new Cookie(Lbinfologin, _sessionInfo.Lbinfologin));
            var api = SubDomain + $@"/credit-and-mortgage/mortgages/{loanId}?accountId={account.BankNumber}-{account.BranchNumber}-{account.AccountNumber}&accountNumber={account.AccountNumber}&bankNumber={account.BankNumber}&branchNumber={account.BranchNumber}";

            var accountsResponse = CallGetRequest(baseAddress, api, cookies, null);
            if (string.IsNullOrEmpty(accountsResponse))
            {
                return new HapoalimMortgageAssetResponse();
            }

            var content = JsonConvert.DeserializeObject<HapoalimMortgageAssetResponse>(accountsResponse);
            return content;
        }

        public HapoalimLoansResponse GetLoans(BankAccount account)
        {
            var baseAddress = new Uri(LoginDomain);
            var cookies = new CookieContainer();
            cookies.Add(baseAddress, new Cookie(ActiveUser, _sessionInfo.ActiveUser));
            cookies.Add(baseAddress, new Cookie(Smsession, _sessionInfo.Smsession));
            cookies.Add(baseAddress, new Cookie(Jsessionid, _sessionInfo.Jsessionid));
            cookies.Add(baseAddress, new Cookie(Token, _sessionInfo.Token));
            cookies.Add(baseAddress, new Cookie(Lbinfologin, _sessionInfo.Lbinfologin));
            var api = SubDomain + $@"/credit-and-mortgage/loans?accountId={account.BankNumber}-{account.BranchNumber}-{account.AccountNumber}";

            var accountsResponse = CallGetRequest(baseAddress, api, cookies, null);
            if (string.IsNullOrEmpty(accountsResponse))
            {
                return new HapoalimLoansResponse();
            }

            var content = JsonConvert.DeserializeObject<HapoalimLoansResponse>(accountsResponse);
            return content;
        }

        public HapoalimLoanDetailsResponse GetDetailsForLoan(BankAccount account, HapoalimLoansResponse.LoanData loan)
        {
            var baseAddress = new Uri(LoginDomain);
            var cookies = new CookieContainer();
            cookies.Add(baseAddress, new Cookie(ActiveUser, _sessionInfo.ActiveUser));
            cookies.Add(baseAddress, new Cookie(Smsession, _sessionInfo.Smsession));
            cookies.Add(baseAddress, new Cookie(Jsessionid, _sessionInfo.Jsessionid));
            cookies.Add(baseAddress, new Cookie(Token, _sessionInfo.Token));
            cookies.Add(baseAddress, new Cookie(Lbinfologin, _sessionInfo.Lbinfologin));
            var api = SubDomain + $@"/credit-and-mortgage/loans/{loan.CreditSerialNumber}?accountId={account.BankNumber}-{account.BranchNumber}-{account.AccountNumber}&unitedCreditTypeCode={loan.UnitedCreditTypeCode}";

            var accountsResponse = CallGetRequest(baseAddress, api, cookies, null);
            if (string.IsNullOrEmpty(accountsResponse))
            {
                return new HapoalimLoanDetailsResponse();
            }

            var content = JsonConvert.DeserializeObject<HapoalimLoanDetailsResponse>(accountsResponse);
            return content;
        }

        public Decimal GetBalance(BankAccount account)
        {
            var baseAddress = new Uri(LoginDomain);
            var cookies = new CookieContainer();
            cookies.Add(baseAddress, new Cookie(ActiveUser, _sessionInfo.ActiveUser));
            cookies.Add(baseAddress, new Cookie(Smsession, _sessionInfo.Smsession));
            cookies.Add(baseAddress, new Cookie(Jsessionid, _sessionInfo.Jsessionid));
            cookies.Add(baseAddress, new Cookie(Token, _sessionInfo.Token));
            cookies.Add(baseAddress, new Cookie(Lbinfologin, _sessionInfo.Lbinfologin));

            var api = SubDomain + $"/current-account/composite/balanceAndCreditLimit?accountId=" +
                      $"{account.BankNumber}-{account.BranchNumber}-{account.AccountNumber}";

            var balanceResponse = CallGetRequest(baseAddress, api, cookies, null);
            var content = JsonConvert.DeserializeObject<HapoalimBalanceResponse>(balanceResponse);
            return content?.CurrentBalance ?? 0;
        }

        #region Private Methods
        private HapoalimInitLoginResponse Init(String userId)
        {
            var baseAddress = new Uri(LoginDomain);
            IEnumerable<KeyValuePair<string, string>> body = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("identifier", userId),
                new KeyValuePair<string, string>("organization", DefaultOrganisation)
            };

            var content = new FormUrlEncodedContent(body);
            var result = CallPostRequest<HapoalimInitLoginResponse>(baseAddress, "/authenticate/init", content, null, null);
            return result;
        }

        private HapoalimVerifyResponse Verify(String userId, String userCredentials)
        {
            var baseAddress = new Uri(LoginDomain);
            IEnumerable<KeyValuePair<string, string>> body = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("identifier", userId),
                new KeyValuePair<string, string>("Language", ""),
                new KeyValuePair<string, string>("bsd", ""),
                new KeyValuePair<string, string>("userID", userId),
                new KeyValuePair<string, string>("instituteCode", DefaultOrganisation),
                new KeyValuePair<string, string>("credentials", userCredentials),
                new KeyValuePair<string, string>("organization", DefaultOrganisation),
                new KeyValuePair<string, string>("G3CmE", ""),
                new KeyValuePair<string, string>("authType", "VERSAFE"),
                new KeyValuePair<string, string>("flow", ""),
                new KeyValuePair<string, string>("state", ""),
            };

            var content = new FormUrlEncodedContent(body);
            var result = CallPostRequest<HapoalimVerifyResponse>(baseAddress, "/authenticate/verify", content, null, null);
            if (result == null || result.Error != null)
            {
                throw new FieldAccessException(result?.Error.ErrorDescription);
            }
            return result;
        }

        private void GetLandpage()
        {
            var baseAddress = new Uri(LoginDomain);
            var cookies = new CookieContainer();
            cookies.Add(baseAddress, new Cookie(ActiveUser, _sessionInfo.ActiveUser));
            cookies.Add(baseAddress, new Cookie(Smsession, _sessionInfo.Smsession));
            cookies.Add(baseAddress, new Cookie(Lbinfologin, _sessionInfo.Lbinfologin));
            var api = "/AUTHENTICATE/LANDPAGE?flow=AUTHENTICATE&state=LANDPAGE&reqName=MainFrameSet";

            string result = CallGetRequest(baseAddress, api, cookies, null);
        }

        protected override void Exit()
        {
            var baseAddress = new Uri(LoginDomain);
            var cookies = new CookieContainer();
            cookies.Add(baseAddress, new Cookie(ActiveUser, _sessionInfo.ActiveUser));
            cookies.Add(baseAddress, new Cookie(Smsession, _sessionInfo.Smsession));
            cookies.Add(baseAddress, new Cookie(Jsessionid, _sessionInfo.Jsessionid));
            cookies.Add(baseAddress, new Cookie(Token, _sessionInfo.Token));
            cookies.Add(baseAddress, new Cookie(Lbinfologin, _sessionInfo.Lbinfologin));
            var api = "/cgi-bin/poalwwwc?reqName=Logoff";

            var response = CallGetRequest(baseAddress, api, cookies, null);
        }

        #endregion

        protected override void ExtractCookies(HttpResponseMessage response)
        {
            if (_sessionInfo == null)
            {
                _sessionInfo = new HapoalimSessionInfo();
            }

            response.Headers.TryGetValues("set-cookie", out var setValues);
            if (setValues == null)
            {
                return;
            }

            foreach (var setValue in setValues)
            {
                var items = setValue.Split(';');
                foreach (var item in items)
                {
                    var nameValue = item.Split('=');

                    if (nameValue[0].Equals(Smsession))
                    {
                        _sessionInfo.Smsession = nameValue[1];
                    }
                    else if (nameValue[0].Equals(Lbinfologin))
                    {
                        _sessionInfo.Lbinfologin = nameValue[1];
                    }
                    else if (nameValue[0].Equals(ActiveUser))
                    {
                        _sessionInfo.ActiveUser = nameValue[1];
                    }
                    else if (nameValue[0].Equals(Jsessionid))
                    {
                        _sessionInfo.Jsessionid = nameValue[1];
                    }
                    else if (nameValue[0].Equals(Token))
                    {
                        _sessionInfo.Token = nameValue[1];
                    }
                }
            }
        }

        protected override void ExitErrorHandler(Task task, object context)
        {
            //throw new NotImplementedException();
        }
    }
}
