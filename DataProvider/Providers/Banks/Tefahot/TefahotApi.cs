using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using DataProvider.Providers.Banks.Tefahot.Dto;
using DataProvider.Providers.Models;
using GoldMountainShared.Storage.Documents;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Transaction = DataProvider.Providers.Models.Transaction;

namespace DataProvider.Providers.Banks.Tefahot
{
    public class TefahotApi : ITefahotApi
    {
        private readonly String _userName;
        private readonly String _userPassword;

        private const string ViewState = "__VIEWSTATE";
        private const string ViewStateGenerator = "__VIEWSTATEGENERATOR";
        private const string EventValidation = "__EVENTVALIDATION";
        
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

            var viewstate = Login();
            RetriveSession(viewstate);
        }

        public IEnumerable<TefahotProfileResponse.AccountProfile> GetAccounts()
        {
            if (String.IsNullOrEmpty(_sessionInfo.MizSession))
            {
                return new List<TefahotProfileResponse.AccountProfile>();
            }

            var profileResponse = Logon();
            var accountsProfile = JsonConvert.DeserializeObject<TefahotProfileResponse>(profileResponse);
            var accounts = accountsProfile.Body.User.Accounts;
            _sessionInfo.XsrfToken = accountsProfile.Body.XsrfToken;

            return accounts;
        }

        public IEnumerable<Transaction> GetTransactions(string accountId, DateTime startTime, DateTime endTime)
        {
            if (String.IsNullOrEmpty(_sessionInfo.MizSession))
            {
                return new List<Transaction>();
            }

            LoadOshPage();

            var result = LoadOshTransactions(startTime, endTime);
            var transacations = GetTransactionsData(result).ToList();
            while (result.Contains("javascript:__doPostBack(&#39;ctl00$ContentPlaceHolder2$uc428Grid$grvP428G2$ctl00$ctl02$ctl00$ctl04"))
            {
                result = LoadOshNextTransactions(startTime, endTime);
                transacations.AddRange(GetTransactionsData(result));
            }

            return transacations;
        }

        private static IEnumerable<Transaction> GetTransactionsData(string data)
        {
            var result = new List<Transaction>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(data);

            var table = doc.DocumentNode.Descendants("table").FirstOrDefault(a =>
                a.Attributes.Contains("id") &&
                a.Attributes["id"].Value.Equals("ctl00_ContentPlaceHolder2_uc428Grid_grvP428G2_ctl00"));

            if (table == null)
            {
                return result;
            }

            foreach (var row in table.SelectNodes("tbody").Nodes())
            {
                if (row.ChildNodes.Count <5) { continue; }
                
                var date = row.SelectNodes("td")[0].InnerText.Split("/");
                if (!int.TryParse(date[0], out int n)) { continue; }

                var amount = Convert.ToDecimal(row.SelectNodes("td")[4].InnerText);
                var balance = row.SelectNodes("td")[5].InnerText;

                var tr = new Transaction();
                tr.PaymentDate = new DateTime(2000 + Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0]));
                tr.PurchaseDate = tr.PaymentDate;
                tr.Type = amount > 0 ? TransactionType.Income : TransactionType.Expense;
                tr.Amount = Math.Abs(amount);
                tr.Description = row.SelectNodes("td")[3].InnerText;
                tr.CurrentBalance = String.IsNullOrEmpty(balance) ? Decimal.MinValue : Convert.ToDecimal(balance);
                tr.Id = (long)(Math.Round(tr.Amount) + Math.Round(tr.CurrentBalance.Equals(Decimal.MinValue) ? 0 : tr.CurrentBalance));
                
                result.Add(tr);
            }

            return result;
        }

        public IEnumerable<TefahotMortgagesResponse.TefahotMortgage> GetMortgages(string accountId)
        {
            var headers = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36"),
                new Tuple<string, string>("Referer", "https://mto.mizrahi-tefahot.co.il/ngOnline/index.html"),
                new Tuple<string, string>("mizrahixsrftoken", _sessionInfo.XsrfToken),
            };

            var cookies = new CookieContainer();
            cookies.Add(_mtoAddress, new Cookie(AspSession, _sessionInfo.AspSession));
            cookies.Add(_mtoAddress, new Cookie(MizSession, _sessionInfo.MizSession));

            var result = CallGetRequest(_mtoAddress, "/Online/api/mashkanta/profile", cookies, headers);
            var mortgages = JsonConvert.DeserializeObject<TefahotMortgagesResponse>(result);

            foreach (var mortgage in mortgages.Mashkantaot)
            {
                var loans = GetMortgageLoans(mortgage.Id);
                mortgage.Maslolim = loans.Maslolim;
            }

            return mortgages.Mashkantaot;
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
            Logout();
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

        private void Logout()
        {
            String body = "{\"fromExitLink\":true}";

            var headers = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36"),
                new Tuple<string, string>("Referer", "https://mto.mizrahi-tefahot.co.il/ngOnline/index.html"),
            };

            var cookieContainer = new CookieContainer();
            cookieContainer.Add(_mtoAddress, new Cookie(AspSession, _sessionInfo.AspSession));
            cookieContainer.Add(_mtoAddress, new Cookie(MizSession, _sessionInfo.MizSession));

            CallPostRequest(_mtoAddress, "/Online/api/newGE/endSession ",
                new StringContent(body, Encoding.UTF8, "application/json"),
                cookieContainer, headers);
        }

        private TefahotMortgageLoansResponse GetMortgageLoans(string mortgageId)
        {
            var headers = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36"),
                new Tuple<string, string>("Referer", "https://mto.mizrahi-tefahot.co.il/ngOnline/index.html"),
                new Tuple<string, string>("mizrahixsrftoken", _sessionInfo.XsrfToken),
            };

            var cookies = new CookieContainer();
            cookies.Add(_mtoAddress, new Cookie(AspSession, _sessionInfo.AspSession));
            cookies.Add(_mtoAddress, new Cookie(MizSession, _sessionInfo.MizSession));

            var result = CallGetRequest(_mtoAddress, "/Online/api/mashkantaot/details/" + mortgageId + "/maslolim", cookies, headers);
            var mortgages = JsonConvert.DeserializeObject<TefahotMortgageLoansResponse>(result);
            return mortgages;
        }

        private String LoadOshTransactions(DateTime startTime, DateTime endTime)
        {
            var body = GetFirstOshRequestBody(startTime, endTime);

            var headers = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("User-Agent",
                    "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36"),
                new Tuple<string, string>("Referer", "https://mto.mizrahi-tefahot.co.il/Online/Osh/P428.aspx"),
            };

            var cookies = new CookieContainer();
            cookies.Add(_mtoAddress, new Cookie(AspSession, _sessionInfo.AspSession));
            cookies.Add(_mtoAddress, new Cookie(MizSession, _sessionInfo.MizSession));

            var responseBody =  CallPostRequest(_mtoAddress, "/Online/Osh/P428.aspx", new FormUrlEncodedContent(body), cookies, headers);

            _sessionInfo.ViewState = ExtractAspEntity(responseBody, ViewState);
            _sessionInfo.ViewStateGenerator = ExtractAspEntity(responseBody, ViewStateGenerator);
            _sessionInfo.EventValidation = ExtractAspEntity(responseBody, EventValidation);

            return responseBody;
        }

        private String LoadOshNextTransactions(DateTime startTime, DateTime endTime)
        {
            var body = GetNextOshRequestBody(startTime, endTime);
            var headers = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36"),
            };

            var cookies = new CookieContainer();
            cookies.Add(_mtoAddress, new Cookie(AspSession, _sessionInfo.AspSession));
            cookies.Add(_mtoAddress, new Cookie(MizSession, _sessionInfo.MizSession));

            var responseBody = CallPostRequest(_mtoAddress, "/Online/Osh/P428.aspx", new FormUrlEncodedContent(body), cookies, headers);

            _sessionInfo.ViewState = ExtractAspEntity(responseBody, ViewState);
            _sessionInfo.ViewStateGenerator = ExtractAspEntity(responseBody, ViewStateGenerator);
            _sessionInfo.EventValidation = ExtractAspEntity(responseBody, EventValidation);

            return responseBody;
        }

        private void LoadOshPage()
        {
            var headers = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("User-Agent",
                    "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36"),
            };

            var cookies = new CookieContainer();
            cookies.Add(_mtoAddress, new Cookie(AspSession, _sessionInfo.AspSession));
            cookies.Add(_mtoAddress, new Cookie(MizSession, _sessionInfo.MizSession));

            var responseBody = CallGetRequest(_mtoAddress, "/Online/Osh/P428.aspx", cookies, headers);

            _sessionInfo.ViewState = ExtractAspEntity(responseBody, ViewState);
            _sessionInfo.ViewStateGenerator = ExtractAspEntity(responseBody, ViewStateGenerator);
            _sessionInfo.EventValidation = ExtractAspEntity(responseBody, EventValidation);
        }

        private IEnumerable<KeyValuePair<string, string>> GetFirstOshRequestBody(DateTime startTime, DateTime endTime)
        {
            var body = GetBodyForOshRequest(startTime, endTime).ToList();
            body.Add(new KeyValuePair<string, string>("__VIEWSTATE", _sessionInfo.ViewState));
            body.Add(new KeyValuePair<string, string>("__VIEWSTATEGENERATOR", _sessionInfo.ViewStateGenerator));
            body.Add(new KeyValuePair<string, string>("__EVENTVALIDATION", _sessionInfo.EventValidation));
            body.Add(new KeyValuePair<string, string>("__EVENTTARGET", ""));
            body.Add(new KeyValuePair<string, string>("ctl00$ContentPlaceHolder2$btShow", "הצג"));

            return body;
        }

        private IEnumerable<KeyValuePair<string, string>> GetNextOshRequestBody(DateTime startTime, DateTime endTime)
        {
            var body = GetBodyForOshRequest(startTime, endTime).ToList();
            body.Add(new KeyValuePair<string, string>("__VIEWSTATE", _sessionInfo.ViewState));
            body.Add(new KeyValuePair<string, string>("__VIEWSTATEGENERATOR", _sessionInfo.ViewStateGenerator));
            body.Add(new KeyValuePair<string, string>("__EVENTVALIDATION", _sessionInfo.EventValidation));
            body.Add(new KeyValuePair<string, string>("__EVENTTARGET", "ctl00$ContentPlaceHolder2$uc428Grid$grvP428G2$ctl00$ctl02$ctl00$ctl04"));
            body.Add(new KeyValuePair<string, string>("__ASYNCPOST", "true"));
            body.Add(new KeyValuePair<string, string>("ctl00$radScriptManager", "ctl00$ContentPlaceHolder2$ctl00$ContentPlaceHolder2$uc428Grid$grvP428G2Panel|ctl00$ContentPlaceHolder2$uc428Grid$grvP428G2$ctl00$ctl02$ctl00$ctl04"));
            body.Add(new KeyValuePair<string, string>("RadAJAXControlID", "ctl00_ContentPlaceHolder2_uc428Grid_PageAjaxPanel"));
            body.Add(new KeyValuePair<string, string>("RadAJAXControlID", "ctl00_ContentPlaceHolder2_PageAjaxManager"));
            

            return body;
        }

        private IEnumerable<KeyValuePair<string, string>> GetBodyForOshRequest(DateTime startTime, DateTime endTime)
        {
            var start = $"{startTime.Year}, {startTime.Month}, {startTime.Day}";
            var end = $"{endTime.Year}, {endTime.Month}, {endTime.Day}";
            var start2 = $"{startTime.Year}-{startTime.Month:D2}-{startTime.Day:D2}";
            var end2 = $"{endTime.Year}-{endTime.Month:D2}-{endTime.Day:D2}";
            var start3 = $"{startTime.Day:D2}/{startTime.Month:D2}/{startTime.Year}";
            var end3 = $"{endTime.Day:D2}/{endTime.Month:D2}/{endTime.Year}";

            IEnumerable<KeyValuePair<string, string>> body = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ctl00_radScriptManager_TSM", ";;Telerik.Web.UI, Version=2013.2.717.45, Culture=neutral, PublicKeyToken=121fae78165ba3d4:en-US:4401a8f1-5215-4b97-a426-3601ce0fa0ff:16e4e7cd:ed16cbdc:365331c3:7c926187:8674cba1:b7778d6c:c08e9f8a:59462f1:a51ee93e:58366029"),
                new KeyValuePair<string, string>("ctl00_RadStyleSheetManager_TSSM", ""),
                
                new KeyValuePair<string, string>("__EVENTARGUMENT", ""),
                new KeyValuePair<string, string>("__LASTFOCUS", ""),
                new KeyValuePair<string, string>("__VIEWSTATEENCRYPTED", ""),
                new KeyValuePair<string, string>("ctl00$ContentPlaceHolder2$ddlRules", "0"),
                new KeyValuePair<string, string>("ctl00$ContentPlaceHolder2$SkyDRP$SkyDatePicker1ID$radDatePickerID", start2),
                new KeyValuePair<string, string>("ctl00$ContentPlaceHolder2$SkyDRP$SkyDatePicker1ID$radDatePickerID$dateInput", start3),
                new KeyValuePair<string, string>("ctl00_ContentPlaceHolder2_SkyDRP_SkyDatePicker1ID_radDatePickerID_dateInput_ClientState", "{\"enabled\":true,\"emptyMessage\":\"\",\"validationText\":\"" + start2 + "-00-00-00\",\"valueAsString\":\"" + start2 + "-00-00-00\",\"minDateStr\":\"" + start2 + "-00-00-00\",\"maxDateStr\":\"" + end2 + "-00-00-00\",\"lastSetTextBoxValue\":\"" + start3 + "\"}"),
                new KeyValuePair<string, string>("ctl00_ContentPlaceHolder2_SkyDRP_SkyDatePicker1ID_radDatePickerID_calendar_SD", "[[" + start +"]]"),
                new KeyValuePair<string, string>("ctl00_ContentPlaceHolder2_SkyDRP_SkyDatePicker1ID_radDatePickerID_calendar_AD", "[[" + start +"],[" + end +"],[" + start +"]]"),
                new KeyValuePair<string, string>("ctl00_ContentPlaceHolder2_SkyDRP_SkyDatePicker1ID_radDatePickerID_ClientState", "{\"minDateStr\":\"" + start2 + "-00-00-00\",\"maxDateStr\":\"" + end2 + "-00-00-00\"}"),
                new KeyValuePair<string, string>("ctl00$ContentPlaceHolder2$SkyDRP$SkyDatePicker2ID$radDatePickerID", end2),
                new KeyValuePair<string, string>("ctl00$ContentPlaceHolder2$SkyDRP$SkyDatePicker2ID$radDatePickerID$dateInput", end3),
                new KeyValuePair<string, string>("ctl00_ContentPlaceHolder2_SkyDRP_SkyDatePicker2ID_radDatePickerID_dateInput_ClientState", "{\"enabled\":true,\"emptyMessage\":\"\",\"validationText\":\"" + end2 + "-00-00-00\",\"valueAsString\":\"" + end2 + "-00-00-00\",\"minDateStr\":\"" + start2 + "-00-00-00\",\"maxDateStr\":\"" + end2 + "-00-00-00\",\"lastSetTextBoxValue\":\"" + end3 + "\"}"),
                new KeyValuePair<string, string>("ctl00_ContentPlaceHolder2_SkyDRP_SkyDatePicker2ID_radDatePickerID_calendar_SD", "[[" + end +"]]"),
                new KeyValuePair<string, string>("ctl00_ContentPlaceHolder2_SkyDRP_SkyDatePicker2ID_radDatePickerID_calendar_AD", "[[" + start +"],[" + end +"],[" + end +"]]"),
                new KeyValuePair<string, string>("ctl00_ContentPlaceHolder2_SkyDRP_SkyDatePicker2ID_radDatePickerID_ClientState", "{\"minDateStr\":\"" + start2 + "-00-00-00\",\"maxDateStr\":\"" + end2 + "-00-00-00\"}"),
                new KeyValuePair<string, string>("ctl00$ContentPlaceHolder2$ddlSug", "00"),
                new KeyValuePair<string, string>("ctl00_ContentPlaceHolder2_uc428Grid_grvP428G2_ClientState", ""),
                new KeyValuePair<string, string>("ctl00$hdnLinkToExternalSite", "קישור לאתר חיצוני")
            };
            return body;
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
            const string hiddenDelimiter = "|";
            try
            {
                int viewStateStartPosition, viewStateEndPosition;
                int viewStateNamePosition = data.IndexOf(entity, StringComparison.Ordinal);
                if (viewStateNamePosition == -1)
                {
                    return string.Empty;
                }

                var viewStateValuePosition = data.IndexOf(valueDelimiter, viewStateNamePosition, StringComparison.Ordinal);
                if (viewStateValuePosition == -1)
                {
                    viewStateValuePosition = data.IndexOf(hiddenDelimiter, viewStateNamePosition, StringComparison.Ordinal);
                    viewStateStartPosition = viewStateValuePosition + hiddenDelimiter.Length;
                    viewStateEndPosition = data.IndexOf(hiddenDelimiter, viewStateStartPosition, StringComparison.Ordinal);
                    return data.Substring(viewStateStartPosition, viewStateEndPosition - viewStateStartPosition);
                }

                viewStateStartPosition = viewStateValuePosition + valueDelimiter.Length;
                viewStateEndPosition = data.IndexOf("\"", viewStateStartPosition, StringComparison.Ordinal);
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
            public string XsrfToken { get; set; }

            public string ViewState { get; set; }
            public string ViewStateGenerator { get; set; }
            public string EventValidation { get; set; }
        }
    }
}


