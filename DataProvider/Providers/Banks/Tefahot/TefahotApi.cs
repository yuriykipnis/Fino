using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using DataProvider.Providers.Banks.Tefahot.Dto;
using GoldMountainShared.Storage.Documents;
using Newtonsoft.Json;
using BankAccount = DataProvider.Providers.Models.BankAccount;
using Mortgage = DataProvider.Providers.Models.Mortgage;
using Transaction = DataProvider.Providers.Models.Transaction;

namespace DataProvider.Providers.Banks.Tefahot
{
    public class TefahotApi : ITefahotApi
    {
        private readonly String _userName;
        private readonly String _userPassword;

        private const string ViewState = "__VIEWSTATE";
        private const string ViewStateGenerator = "__VIEWSTATEGENERATOR";
        private const string MizSession = "MizSESSION";
        private const string AspSession = "ASP.NET_SessionId";

        private SessionInfo _sessionInfo;
        private readonly Uri _baseAddress = new Uri("https://www.mizrahi-tefahot.co.il");
        private readonly Uri _mtoAddress = new Uri("https://mto.mizrahi-tefahot.co.il");

        public TefahotApi(Provider providerDescriptor)
        {
            if (providerDescriptor == null || providerDescriptor.Credentials.Count == 0)
            {
                throw new ArgumentNullException(nameof(providerDescriptor));
            }

            var crentialValues = providerDescriptor.Credentials.Values.ToArray();
            _userName = crentialValues[0];
            _userPassword = crentialValues[1];
        }

        public IEnumerable<TefahotProfileResponse.AccountProfile> GetAccounts()
        {
            var viewstate = Login();
            RetriveSession(viewstate);
            if (String.IsNullOrEmpty(_sessionInfo.MizSession))
            {
                return new List<TefahotProfileResponse.AccountProfile>();
            }

            var profileResponse = Logon();
            var accountsProfile = JsonConvert.DeserializeObject<TefahotProfileResponse>(profileResponse);

            return accountsProfile.Body.User.Accounts;
        }

        private Tuple<String, String> Login()
        {
            IEnumerable<KeyValuePair<string, string>> body = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Username", _userName),
                new KeyValuePair<string, string>("Password", _userPassword),
                new KeyValuePair<string, string>("Lang", "HE")
            };

            var cookies = new CookieContainer();
            cookies.Add(_baseAddress, new Cookie(AspSession, ""));

            var headers = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("Referer", "https://www.mizrahi-tefahot.co.il/he/bank/Pages/Default.aspx"),
            };

            var responseBody = CallPostRequest(_baseAddress, "/login/login2MTO.aspx", new FormUrlEncodedContent(body), cookies, headers, LoginRedirectHandler);
            var stateView = ExtractAspEntity(responseBody, ViewState);
            var viewStateGenerator = ExtractAspEntity(responseBody, ViewStateGenerator);
            return new Tuple<string, string>(stateView, viewStateGenerator);
        }

        private String LoginRedirectHandler(String redirectUri)
        {
            var headers = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("Referer", "https://www.mizrahi-tefahot.co.il/he/bank/Pages/Default.aspx"),
            };

            var cookies = new CookieContainer();
            cookies.Add(_baseAddress, new Cookie(AspSession, _sessionInfo.AspSession));

            return CallGetRequest(_baseAddress, redirectUri, cookies, headers);
        }

        private String RetriveSession(Tuple<string, string> stateView)
        {
            IEnumerable<KeyValuePair<string, string>> body = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(ViewState, stateView.Item1),
                new KeyValuePair<string, string>(ViewStateGenerator, stateView.Item2)
            };

            var cookies = new CookieContainer();
            cookies.Add(_baseAddress, new Cookie(AspSession, _sessionInfo.AspSession));

            var headers = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("Referer", "https://www.mizrahi-tefahot.co.il/login/MiddlePage.aspx"),
            };

            return CallPostRequest(_baseAddress, "/login/MiddlePage.aspx", new FormUrlEncodedContent(body), cookies, headers, RetriveSessionRedirectHandler);
        }

        private String RetriveSessionRedirectHandler(String redirectUri)
        {
            var headers = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("Cache-Control", "max-age=0"),
                new Tuple<string, string>("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36"),
                new Tuple<string, string>("Referer", "https://www.mizrahi-tefahot.co.il/login/MiddlePage.aspx"),
            };

            var cookies = new CookieContainer();
            cookies.Add(_mtoAddress, new Cookie(MizSession, _sessionInfo.MizSession));

            return CallGetRequest(_baseAddress, redirectUri, cookies, headers);
        }

        private String Logon()
        {
            String body = "{\"appId\":\"skyWeb\",\"appVer\":\"\",\"lang\":\"he-il\"}";

            var headers = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36"),
            };

            var cookieContainer = new CookieContainer();
            cookieContainer.Add(_mtoAddress, new Cookie(AspSession, _sessionInfo.AspSession));
            cookieContainer.Add(_mtoAddress, new Cookie(MizSession, _sessionInfo.MizSession));

            var result = CallPostRequest(_mtoAddress, "/Online/api/SkyBL/logon", 
                new StringContent(body, Encoding.UTF8, "application/json"), 
                cookieContainer, headers);

            return result;
        }

        public IEnumerable<object> GetTransactions(string accountId, DateTime startTime, DateTime endTime)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetMortgages(string accountId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetBalance(string accountId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetLoans(string accountId)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            
        }

        private string CallGetRequest(Uri domainUri, string api, CookieContainer cookies, IEnumerable<Tuple<string, string>> headers = null)
        {
            HttpResponseMessage response;
            using (var httpClientHandler = new HttpClientHandler{
                CookieContainer = cookies,
            })
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                using (var client = new HttpClient(httpClientHandler) { BaseAddress = domainUri })
                {
                    if (headers != null)
                    {
                        foreach (var header in headers)
                        {
                            client.DefaultRequestHeaders.Add(header.Item1, header.Item2);
                        }
                    }

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

        private String CallPostRequest(Uri domainUri, string api, HttpContent body, 
                                       CookieContainer cookies, IEnumerable<Tuple<string, string>> headers = null, 
                                       Func<string, string> redirectCallback = null)
        {
            HttpResponseMessage response;
            var allowAutoRedirect = redirectCallback == null;
            using (var httpClientHandler = new HttpClientHandler
            {
                CookieContainer = cookies,
                AllowAutoRedirect = allowAutoRedirect
            })
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                using (var client = new HttpClient(httpClientHandler) { BaseAddress = domainUri })
                {
                    if (headers != null)
                    {
                        foreach (var header in headers)
                        {
                            client.DefaultRequestHeaders.Add(header.Item1, header.Item2);
                        }
                    }

                    response = client.PostAsync(api, body).Result;
                }
            }

            if (!allowAutoRedirect)
            {
                var statusCode = (int)response.StatusCode;
                if (statusCode >= 300 && statusCode <= 399)
                {
                    ExtractCookies(response);
                    return redirectCallback(response.Headers.Location.ToString());
                }
            }

            if (!response.IsSuccessStatusCode)
            {
                return String.Empty;
            }

            ExtractCookies(response);

            String result;
            try
            {
                 result = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return String.Empty;
            }
            return result;
        }

        private void ExtractCookies(HttpResponseMessage response)
        {
            if (_sessionInfo == null)
            {
                _sessionInfo = new SessionInfo();
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

                    if (nameValue[0].Equals(MizSession))
                    {
                        _sessionInfo.MizSession = nameValue[1];
                    }
                    else if (nameValue[0].Equals(AspSession))
                    {
                        _sessionInfo.AspSession = nameValue[1];
                    }
                }
            }
        }

        private static string ExtractAspEntity(string data, string entity)
        {
            const string valueDelimiter = "value=\"";
            try
            {
                int viewStateNamePosition = data.IndexOf(entity, StringComparison.Ordinal);
                if (viewStateNamePosition == -1)
                {
                    return string.Empty;
                }

                int viewStateValuePosition = data.IndexOf(valueDelimiter, viewStateNamePosition, StringComparison.Ordinal);
                int viewStateStartPosition = viewStateValuePosition + valueDelimiter.Length;
                int viewStateEndPosition = data.IndexOf("\"", viewStateStartPosition, StringComparison.Ordinal);
                return data.Substring(viewStateStartPosition, viewStateEndPosition - viewStateStartPosition);
            }
            catch (Exception exp)
            {
            }

            return string.Empty;
        }

        private class SessionInfo
        {
            public string MizSession { get; set; }
            public string AspSession { get; set; }
        }
    }
}
