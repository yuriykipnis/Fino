using System;

namespace DataProvider.Providers.Exceptions
{
    public class LoginException : Exception
    {
        protected string ProviderName { get; set; }
        protected string Username { get; set; }
        public string Error { get; set; }

        public LoginException(string providerName, string username) :
            base(ErrorMessage(providerName, username))
        {
            ProviderName = providerName;
            Username = username;
        }

        public LoginException(string providerName, string username, Exception inner) : 
            base(ErrorMessage(providerName, username), inner)
        {
        }

        private static String ErrorMessage(string providerName, string username)
        {
            return $"Failed to login to {providerName} for {username}";
        }
    }
}
