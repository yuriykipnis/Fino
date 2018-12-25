using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProvider.Providers.Banks.Tefahot.Dto
{
    public class TefahotMortgagesResponse
    {
        public Decimal SachAmlotLeSiluk { get; set; }
        public Decimal SachItraLeSiluk { get; set; }
        public Decimal RibitPrime { get; set; }
        public Decimal SachSchumHiuvHodshiAharon { get; set; }

        public IEnumerable<TefahotMortgage> Mashkantaot { get; set; } = new List<TefahotMortgage>();

        public class TefahotMortgage
        {
            public String Id { get; set; }
            public Decimal ItraAmlotSiluk { get; set; }
            public Decimal ItraLeSiluk { get; set; }
            public String KtovetNehes { get; set; }
            public String Matara { get; set; }
            public Decimal Schum { get; set; }
            public Decimal SchumHiuvAba { get; set; }
            public String TaarihHiuvAba { get; set; }
            public Decimal RibitPrime { get; set; }

            public IEnumerable<TefahotMortgageLoansResponse.MortgageLoans> Maslolim { get; set; }
        }

    }
}
