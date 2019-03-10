using System;

namespace DataProvider.Providers.Exceptions
{
    public class HttpResponseErrorException : Exception
    {
        public string Error { get; set; }

        public HttpResponseErrorException(string api, string error) :
            base(ErrorMessage(api, error))
        {
        }

        public HttpResponseErrorException(string api, string error, Exception inner) :
            base(ErrorMessage(api, error), inner)
        {
        }

        private static String ErrorMessage(string api, string error)
        {
            return $"Call to {api} succeed but response parsing failed with error: {error}";
        }
    }
}