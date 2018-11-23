using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataProvider.Providers.Cards.Amex.Dto;
using GoldMountainShared.Storage.Documents;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataProvider.Providers.Cards.Amex
{
    public class AmexApi : IAmexApi
    {
        #region Constants
        private const string LoginDomain = "https://he.americanexpress.co.il";
        private const string AspDotNetSessionId = "ASP.NET_SessionId";
        private const string Jsessionid = "JSESSIONID";
        private const string Alt50_ZLinuxPrd = "Alt50_ZLinuxPrd";
        private const string ServiceP = "ServiceP";
        private const string RequestVerificationToken = "__RequestVerificationToken";
        #endregion

        #region Fields
        private AmexSessionInfo _sessionInfo;
        #endregion

        public AmexApi(Provider providerDescriptor)
        {
            if (providerDescriptor == null || providerDescriptor.Credentials.Count == 0)
            {
                throw new ArgumentNullException(nameof(providerDescriptor));
            }

            var crentialValues = providerDescriptor.Credentials.Values.ToArray();
            var idNumber = crentialValues[0];
            var lastDigits = crentialValues[1];
            var password = crentialValues[2];

            GetVerificationToken();
            ValidateId(idNumber, lastDigits);

            var loginResponse = Login(idNumber, lastDigits, password);
        }

        public CardListDeatils GetCards()
        {
            var api = "/services/ProxyRequestHandler.ashx?reqName=CardsList_102Digital";
            var baseAddress = new Uri(LoginDomain);

            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseAddress, new Cookie(Jsessionid, _sessionInfo.Jsessionid));
            cookieContainer.Add(baseAddress, new Cookie(Alt50_ZLinuxPrd, _sessionInfo.Alt50_ZLinuxPrd));
            cookieContainer.Add(baseAddress, new Cookie(ServiceP, _sessionInfo.ServiceP));
            cookieContainer.Add(baseAddress, new Cookie(RequestVerificationToken, _sessionInfo.RequestVerificationToken));
            cookieContainer.Add(baseAddress, new Cookie(AspDotNetSessionId, _sessionInfo.AspDotNetSessionId));

            var headersToAdd = new List<KeyValuePair<string, IEnumerable<string>>>
            {
                new KeyValuePair<string, IEnumerable<string>> (RequestVerificationToken, new List<string> { _sessionInfo.AntiForgeryToken })
            };
            
            var response = CallGetRequest(baseAddress, api, cookieContainer, headersToAdd);
            var data = JsonConvert.DeserializeObject<CardListResponse>(response);
            if (IsError(data.Header))
            {
                throw new Exception($"Cannot to get cards. Error: {GetError(data.Header)}");
            }

            return data.CardsList_102DigitalBean;
        }

        public IEnumerable<CardTransaction> GetTransactions(long cardIndex, int month, int year)
        {
            var effMonth = GetEffectiveMonth(month);
            var effYear = GetEffectiveYear(month, year);
            var arguments = $"&month={effMonth:00}&year={effYear}&cardIdx={cardIndex}";
            var api = "/services/ProxyRequestHandler.ashx?reqName=CardsTransactionsList" + arguments;
            var baseAddress = new Uri(LoginDomain);

            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseAddress, new Cookie(Jsessionid, _sessionInfo.Jsessionid));
            cookieContainer.Add(baseAddress, new Cookie(Alt50_ZLinuxPrd, _sessionInfo.Alt50_ZLinuxPrd));
            cookieContainer.Add(baseAddress, new Cookie(ServiceP, _sessionInfo.ServiceP));
            cookieContainer.Add(baseAddress, new Cookie(RequestVerificationToken, _sessionInfo.RequestVerificationToken));
            cookieContainer.Add(baseAddress, new Cookie(AspDotNetSessionId, _sessionInfo.AspDotNetSessionId));

            var headersToAdd = new List<KeyValuePair<string, IEnumerable<string>>>
            {
                new KeyValuePair<string, IEnumerable<string>> (RequestVerificationToken, new List<string> { _sessionInfo.AntiForgeryToken })
            };

            var response = CallGetRequest(baseAddress, api, cookieContainer, headersToAdd);
            var expensesInfo = RetriveExpensesInfo(response);
            return expensesInfo.Transactions;
        }

        #region Private Methods
        private int GetEffectiveMonth(int month)
        {
            return month == 12 ? 1 : month + 1;
        }

        private int GetEffectiveYear(int month, int year)
        {
            return month == 12 ? year + 1 : year;
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

        internal static ExpensesInfo RetriveExpensesInfo(string response)
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

            var data = JsonConvert.DeserializeObject<TransactionsListResponse>(result);
            if (IsError(data.Header))
            {
                throw new Exception($"Cannot to get transactions. Error: {GetError(data.Header)}");
            }
            var transactions = data.CardsTransactionsListBean.Index.CurrentCardTransactions.First().TxnIsrael;
            var charge = data.CardsTransactionsListBean.TotalChargeNis;

            return new ExpensesInfo
            {
                Transactions = transactions ?? new List<CardTransaction>(),
                NextCharge = Convert.ToDecimal(charge)
            };
        }

        private LoginResponse Login(string idNumber, string lastDigits, string password)
        {
            var api = "/services/ProxyRequestHandler.ashx?reqName=performLogonA";
            var baseAddress = new Uri(LoginDomain);
            var body = new LogonBody
            {
                MisparZihuy = idNumber,
                CountryCode = "212",
                IdType = "1",
                Sisma = password,
                CardSuffix = lastDigits
            };

            var headersToAdd = new List<KeyValuePair<string, IEnumerable<string>>>
            {
                new KeyValuePair<string, IEnumerable<string>> (RequestVerificationToken, new List<string> { _sessionInfo.AntiForgeryToken }),
            };

            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseAddress, new Cookie(RequestVerificationToken, _sessionInfo.RequestVerificationToken));
            cookieContainer.Add(baseAddress, new Cookie(Alt50_ZLinuxPrd, _sessionInfo.Alt50_ZLinuxPrd));
            cookieContainer.Add(baseAddress, new Cookie(Jsessionid, _sessionInfo.Jsessionid));
            cookieContainer.Add(baseAddress, new Cookie(ServiceP, _sessionInfo.ServiceP));
            cookieContainer.Add(baseAddress, new Cookie(AspDotNetSessionId, _sessionInfo.AspDotNetSessionId));

            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = CallPostRequest<LoginResponse>(baseAddress, api, content, cookieContainer, headersToAdd);
            if (response.Status != 1)
            {
                throw new UnauthorizedAccessException(String.Concat(response.Message, "\n Go to: \n", LoginDomain, "/personalarea/login/"));
            }

            return response;
        }

        private void GetVerificationToken()
        {
            var baseAddress = new Uri(LoginDomain);
            var api = "/personalarea/login/";

            var html = CallGetRequest(baseAddress, api, new CookieContainer(), new List<KeyValuePair<string, IEnumerable<string>>>());

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var t = doc.DocumentNode.Descendants("input").
                FirstOrDefault(a => a.Attributes.Contains("name") && a.Attributes["name"].Value.Equals(RequestVerificationToken));
            _sessionInfo.AntiForgeryToken = t.Attributes["value"].Value;
        }
        
        private string ValidateId(string idNumber, string lastDigits)
        {
            var api = "/services/ProxyRequestHandler.ashx?reqName=ValidateIdData";
            var baseAddress = new Uri(LoginDomain);

            var body = new ValidateIdRequestBody()
            {
                CardSuffix = lastDigits,
                CheckLevel = "1",
                CompanyCode = "77",
                Id = idNumber,
                IdType = "1",
                CountryCode = "212"
            };
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/x-www-form-urlencoded");
            
            var headersToAdd = new List<KeyValuePair<string, IEnumerable<string>>>
            {
                new KeyValuePair<string, IEnumerable<string>> (RequestVerificationToken, new List<string> { _sessionInfo.AntiForgeryToken })
            };

            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseAddress, new Cookie(RequestVerificationToken, _sessionInfo.RequestVerificationToken));
            cookieContainer.Add(baseAddress, new Cookie(AspDotNetSessionId, _sessionInfo.AspDotNetSessionId));

            var response = CallPostRequest<ValidateIdResponse>(baseAddress, api, content, cookieContainer, headersToAdd);
             if (response == null || IsError(response.Header))
             {
                 throw new Exception($"Cannot to code mishtamesh. Error: {GetError(response != null ? response.Header : new HeaderResponse())}");
             }

             return response.ValidateIdDataBean.UserName;
        }

        private void Exit()
        {
            var api = "/services/ProxyRequestHandler.ashx?reqName=performExit";
            var baseAddress = new Uri(LoginDomain);
            var headersToAdd = new List<KeyValuePair<string, IEnumerable<string>>>
            {
                new KeyValuePair<string, IEnumerable<string>> (RequestVerificationToken, new List<string> { _sessionInfo.AntiForgeryToken })
            };

            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseAddress, new Cookie(Jsessionid, _sessionInfo.Jsessionid));
            cookieContainer.Add(baseAddress, new Cookie(Alt50_ZLinuxPrd, _sessionInfo.Alt50_ZLinuxPrd));
            cookieContainer.Add(baseAddress, new Cookie(ServiceP, _sessionInfo.ServiceP));
            cookieContainer.Add(baseAddress, new Cookie(RequestVerificationToken, _sessionInfo.RequestVerificationToken));
            cookieContainer.Add(baseAddress, new Cookie(AspDotNetSessionId, _sessionInfo.AspDotNetSessionId));

            var response = CallPostRequest<object>(baseAddress, api, null, cookieContainer, headersToAdd);
        }
        
        private static bool IsError(HeaderResponse header)
        {
            return header == null || header.Status != 1;
        }

        private static string GetError(HeaderResponse header)
        {
            if (header.Status == 0)
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(header?.Message))
            {
                return "Unknown error";
            }

            return header.Message;
        }

        private string CallGetRequest(Uri baseAddress, string api, CookieContainer cookieContainer, IList<KeyValuePair<string, IEnumerable<string>>> headersToAdd)
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
                    foreach (var header in headersToAdd)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }

                    response = client.GetAsync(api).Result;
                }
            }

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            ExtractCookies(response);
            return response.Content.ReadAsStringAsync().Result;
        }

        private T CallPostRequest<T>(Uri baseAddress, string api, StringContent content, CookieContainer cookieContainer, IList<KeyValuePair<string, IEnumerable<string>>> headersToAdd)
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
                    foreach (var header in headersToAdd)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }

                    response = client.PostAsync(api, content).Result;
                }
            }

            if (!response.IsSuccessStatusCode)
            {
                return default(T);
            }

            ExtractCookies(response);

            T result;
            try
            {
                result = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return default(T);
            }
            return result;
        }
        
        private void ExtractCookies(HttpResponseMessage response)
        {
            if (_sessionInfo == null)
            {
                _sessionInfo = new AmexSessionInfo();
            }

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
                    else if (nameValue[0].Equals(Alt50_ZLinuxPrd))
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
