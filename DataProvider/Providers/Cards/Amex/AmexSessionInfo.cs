using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProvider.Providers.Cards.Amex
{
    public class AmexSessionInfo
    {
        public string AspDotNetSessionId { get; set; }
        public string Jsessionid { get; set; }
        public string Alt50_ZLinuxPrd { get; set; }
        public string ServiceP { get; set; }
        public string RequestVerificationToken { get; set; }
        public string AntiForgeryToken { get; set; }
    }
}
