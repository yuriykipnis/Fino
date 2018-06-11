using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DataProvider.Providers.Banks.Hapoalim.Dto;
using GoldMountainShared.Storage.Documents;
using Newtonsoft.Json;

namespace DataProvider.Providers.Banks.Hapoalim
{
    public class HapoalimApi : IHapoalimApi
    {
        #region Constants
        private const string DefaultOrganisation = "106402333";
        private const string LoginDomain = "https://login.bankhapoalim.co.il";
        private const string ActiveUser = "activeUser";
        private const string Smsession = "SMSESSION";
        private const string Jsessionid = "JSESSIONID";
        private const string Token = "token";
        private const string Lbinfologin = "lbinfologin";
        #endregion

        #region Fields
        private HapoalimSessionInfo _sessionInfo;
        #endregion

        public HapoalimApi(Provider providerDescriptor)
        {
            if (providerDescriptor == null || providerDescriptor.Credentials.Count == 0)
            {
                throw new ArgumentNullException(nameof(providerDescriptor));
            }

            var crentialValues = providerDescriptor.Credentials.Values.ToArray();
            var userId = crentialValues[0];
            var userCredentials = crentialValues[1];
            
            var initInfo = Init(userId);
            var result = Verify(userId, userCredentials);
           
            GetLandpage();
        }

        public IEnumerable<HapoalimAccountResponse> GetAccountsData()
        {
            var baseAddress = new Uri(LoginDomain);
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseAddress, new Cookie(ActiveUser, _sessionInfo.ActiveUser));
            cookieContainer.Add(baseAddress, new Cookie(Smsession, _sessionInfo.Smsession));
            cookieContainer.Add(baseAddress, new Cookie(Jsessionid, _sessionInfo.Jsessionid));
            cookieContainer.Add(baseAddress, new Cookie(Token, _sessionInfo.Token));
            cookieContainer.Add(baseAddress, new Cookie(Lbinfologin, _sessionInfo.Lbinfologin));
            var api = "/ServerServices/general/accounts";

            IList<HapoalimAccountResponse> content = null;
            var accountsResponse = CallGetRequest(baseAddress, api, cookieContainer);
            content = JsonConvert.DeserializeObject<IList<HapoalimAccountResponse>>(accountsResponse);
            return content;
        }

        public HapoalimTransactionsResponse GetTransactions(HapoalimAccountResponse account, DateTime startTime, DateTime endTime)
        {
            var date1 = string.Format("{0}{1}{2}", startTime.Year, startTime.Month.ToString("00"), startTime.Day.ToString("00"));
            var date2 = string.Format("{0}{1}{2}", endTime.Year, endTime.Month.ToString("00"), endTime.Day.ToString("00"));

            var baseAddress = new Uri(LoginDomain);
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseAddress, new Cookie(ActiveUser, _sessionInfo.ActiveUser));
            cookieContainer.Add(baseAddress, new Cookie(Smsession, _sessionInfo.Smsession));
            cookieContainer.Add(baseAddress, new Cookie(Jsessionid, _sessionInfo.Jsessionid));
            cookieContainer.Add(baseAddress, new Cookie(Token, _sessionInfo.Token));
            cookieContainer.Add(baseAddress, new Cookie(Lbinfologin, _sessionInfo.Lbinfologin));
            var api = $@"/ServerServices/current-account/transactions?accountId={account.BankNumber}-{account.BranchNumber}-{account.AccountNumber}&retrievalEndDate={date2}&retrievalStartDate={date1}";

            var accountsResponse = CallGetRequest(baseAddress, api, cookieContainer);
            if (string.IsNullOrEmpty(accountsResponse)) 
            {
                return new HapoalimTransactionsResponse();
            }

            var content = JsonConvert.DeserializeObject<HapoalimTransactionsResponse>(accountsResponse);
            return content;
        }

        public HapoalimBalanceResponse GetBalance(HapoalimAccountResponse account)
        {
            var baseAddress = new Uri(LoginDomain);
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseAddress, new Cookie(ActiveUser, _sessionInfo.ActiveUser));
            cookieContainer.Add(baseAddress, new Cookie(Smsession, _sessionInfo.Smsession));
            cookieContainer.Add(baseAddress, new Cookie(Jsessionid, _sessionInfo.Jsessionid));
            cookieContainer.Add(baseAddress, new Cookie(Token, _sessionInfo.Token));
            cookieContainer.Add(baseAddress, new Cookie(Lbinfologin, _sessionInfo.Lbinfologin));

            var api = string.Format(@"/ServerServices/current-account/composite/balanceAndCreditLimit?accountId={0}-{1}-{2}",
                account.BankNumber, account.BranchNumber, account.AccountNumber);

            var balanceResponse = CallGetRequest(baseAddress, api, cookieContainer);
            var content = JsonConvert.DeserializeObject<HapoalimBalanceResponse>(balanceResponse);
            return content;
        }

        #region Private Methods
        private InitLoginResponse Init(String userId)
        {
            var baseAddress = new Uri(LoginDomain);
            IEnumerable<KeyValuePair<string, string>> content = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("identifier", userId),
                new KeyValuePair<string, string>("organization", DefaultOrganisation)
            };

            var result = CallPostRequest<InitLoginResponse>(baseAddress, "/authenticate/init", content);
            return result;
        }

        private VerifyResponse Verify(String userId, String userCredentials)
        {
            var baseAddress = new Uri(LoginDomain);
            IEnumerable<KeyValuePair<string, string>> content = new List<KeyValuePair<string, string>>
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

            var result = CallPostRequest<VerifyResponse>(baseAddress, "/authenticate/verify", content);
            if (result == null || result.Error != null)
            {
                throw new FieldAccessException(result?.Error.ErrorDescription);
            }
            return result;
        }

        private void GetLandpage()
        {
            var baseAddress = new Uri(LoginDomain);
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseAddress, new Cookie(ActiveUser, _sessionInfo.ActiveUser));
            cookieContainer.Add(baseAddress, new Cookie(Smsession, _sessionInfo.Smsession));
            cookieContainer.Add(baseAddress, new Cookie(Lbinfologin, _sessionInfo.Lbinfologin));
            var api = "/AUTHENTICATE/LANDPAGE?flow=AUTHENTICATE&state=LANDPAGE&reqName=MainFrameSet";

            string result = CallGetRequest(baseAddress, api, cookieContainer);
        }

        private void Exit()
        {
            var baseAddress = new Uri(LoginDomain);
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseAddress, new Cookie(ActiveUser, _sessionInfo.ActiveUser));
            cookieContainer.Add(baseAddress, new Cookie(Smsession, _sessionInfo.Smsession));
            cookieContainer.Add(baseAddress, new Cookie(Jsessionid, _sessionInfo.Jsessionid));
            cookieContainer.Add(baseAddress, new Cookie(Token, _sessionInfo.Token));
            cookieContainer.Add(baseAddress, new Cookie(Lbinfologin, _sessionInfo.Lbinfologin));
            var api = "/cgi-bin/poalwwwc?reqName=Logoff";

            var response = CallGetRequest(baseAddress, api, cookieContainer);
        }

        private string CallGetRequest(Uri baseAddress, string api, CookieContainer cookieContainer)
        {
            //WebProxy proxy = WebProxy.GetDefaultProxy();
            HttpResponseMessage response;
            using (var httpClientHandler = new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                //Proxy = proxy,
                //UseProxy = true
            })
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                using (var client = new HttpClient(httpClientHandler) { BaseAddress = baseAddress })
                {
                    
                    response = client.GetAsync(api).Result;
                }
            }

            if (!response.IsSuccessStatusCode)
            {
                return String.Empty;
            }

            ExtractCookies(response);
            return response.Content.ReadAsStringAsync().Result;
        }

        private T CallPostRequest<T>(Uri baseAddress, string api, IEnumerable<KeyValuePair<string, string>> content)
        {
            //WebProxy proxy = WebProxy.GetDefaultProxy();
            HttpResponseMessage response;
            using (var httpClientHandler = new HttpClientHandler
            {
                //Proxy = proxy,
                //UseProxy = true
            })
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                using (var client = new HttpClient(httpClientHandler) { BaseAddress = baseAddress })
                {
                    response = client.PostAsync(api, new FormUrlEncodedContent(content)).Result;
                }
            }

            if (!response.IsSuccessStatusCode)
            {
                return default(T);
            }

            ExtractCookies(response);

            return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
        }

        private void ExtractCookies(HttpResponseMessage response)
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
        #endregion

        #region IDisposable
        public void Dispose()
        {
            Task.Factory.StartNew(Exit).ContinueWith(ErrorHandler, TaskContinuationOptions.OnlyOnFaulted);
        }

        private void ErrorHandler(Task task, object context)
        {
            //throw new NotImplementedException();
        }
        #endregion
    }
}
