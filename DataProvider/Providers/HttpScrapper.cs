using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DataProvider.Providers.Exceptions;
using Newtonsoft.Json;

namespace DataProvider.Providers
{
    public abstract class HttpScrapper : IDisposable
    {
        protected string CallGetRequest(Uri baseAddress, string api, CookieContainer cookies,
                                        IList<Tuple<string, string>> headers,
                                        Func<string, string> redirectCallback = null)
        {
            HttpResponseMessage response;
            var allowAutoRedirect = redirectCallback == null;
            using (var httpClientHandler = new HttpClientHandler
            {
                CookieContainer = cookies,
                AllowAutoRedirect = allowAutoRedirect,
                Proxy = null,
                UseProxy = true,
                DefaultProxyCredentials = CredentialCache.DefaultNetworkCredentials
            })
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                using (var client = new HttpClient(httpClientHandler) { BaseAddress = baseAddress })
                {
                    SetHeaders(client, headers);
                    response = client.GetAsync(api).Result;
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

            String result = String.Empty;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Call to {api} failed with error: {response.ReasonPhrase}");
            }

            ExtractCookies(response);

            try
            {
                result = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exp)
            {
                return result;
            }

            return result;
        }

        protected T CallGetRequest<T>(Uri baseAddress, string api, CookieContainer cookies,
                                      IList<Tuple<string, string>> headers)
        {
            HttpResponseMessage response;
            using (var httpClientHandler = new HttpClientHandler
            {
                CookieContainer = cookies,
                Proxy = null,
                UseProxy = true,
                DefaultProxyCredentials = CredentialCache.DefaultNetworkCredentials
            })
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                using (var client = new HttpClient(httpClientHandler) { BaseAddress = baseAddress })
                {
                    SetHeaders(client, headers);
                    response = client.GetAsync(api).Result;
                }
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

            ExtractCookies(response);

            T result;
            try
            {
                result = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception exp)
            {
                throw new HttpResponseErrorException(api, response.Content.ReadAsStringAsync().Result, exp);
            }

            return result;
        }

        protected T CallPostRequest<T>(Uri baseAddress, string api, HttpContent content,
                                       CookieContainer cookies, IList<Tuple<string, string>> headers)
        {
            HttpResponseMessage response;
            using (var httpClientHandler = new HttpClientHandler
            {
                CookieContainer = cookies,
                Proxy = null,
                UseProxy = true,
                DefaultProxyCredentials = CredentialCache.DefaultNetworkCredentials
            })
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                using (var client = new HttpClient(httpClientHandler) { BaseAddress = baseAddress })
                {
                    SetHeaders(client, headers);
                    response = client.PostAsync(api, content).Result;
                }
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

            ExtractCookies(response);

            T result;
            try
            {
                result = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception exp)
            {
                throw new Exception($"Call to {api} succeed but response parsing failed with error: {exp.Message}", exp);
            }

            return result;
        }

        protected async void CallDeleteRequest(Uri baseAddress, string api, 
                                               CookieContainer cookies, IList<Tuple<string, string>> headers)
        {
            HttpResponseMessage response;
            using (var httpClientHandler = new HttpClientHandler
            {
                CookieContainer = cookies,
                Proxy = null,
                UseProxy = true,
                DefaultProxyCredentials = CredentialCache.DefaultNetworkCredentials
            })
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                using (var client = new HttpClient(httpClientHandler) { BaseAddress = baseAddress })
                {
                    SetHeaders(client, headers);
                    response = await client.DeleteAsync(api);
                }
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Call to {api} failed with error: {response.ReasonPhrase}");
            }
        }

        private static void SetHeaders(HttpClient client, IList<Tuple<string, string>> headers)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.Add(header.Item1, header.Item2);
                }
            }
        }

        protected abstract void ExtractCookies(HttpResponseMessage response);

        #region IDisposable

        public void Dispose()
        {
            Task.Factory.StartNew(Exit).ContinueWith(ExitErrorHandler, TaskContinuationOptions.OnlyOnFaulted);
            //Exit();//for tersting
        }

        protected abstract void Exit();

        protected abstract void ExitErrorHandler(Task task, object context);

        #endregion
    }
}
