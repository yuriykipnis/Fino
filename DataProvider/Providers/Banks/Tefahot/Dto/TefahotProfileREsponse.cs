using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProvider.Providers.Banks.Tefahot.Dto
{
    public class TefahotProfileResponse
    {
        public TefahotProfileBody Body { get; set; }

        public class TefahotProfileBody
        {
            public TefahotProfileUser User { get; set; }
            public String XsrfToken { get; set; }
        }

        public class TefahotProfileUser
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string UserId { get; set; }
            public string LandingPageUrl { get; set; }

            public IList<AccountProfile> Accounts { get; set; } = new List<AccountProfile>();
        }
        
        public class AccountProfile
        {
            public string Branch { get; set; }
            public string Number { get; set; }
            public string Name { get; set; }
            public Decimal Remain { get; set; }
        }
    }
}
