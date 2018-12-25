using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProvider.Providers.Banks.Tefahot.Dto
{
    public class TefahotMortgageLoansResponse
    {
        public IEnumerable<MortgageLoans> Maslolim { get; set; }

        public class MortgageLoans
        {
            public Decimal AhuzRbtHiuv { get; set; }
            public Decimal AhuzRbtMetuemet { get; set; }
            public Decimal AmlatIHodaa { get; set; }
            public Decimal AmlatIvun { get; set; }
            public Decimal AmlatPicuiMadad { get; set; }
            public Decimal HatzmadaKrn { get; set; }
            public Decimal HatzmadaRbt { get; set; }
            public Decimal ItraLesiluk { get; set; }
            public Decimal ItratKrnSiluk { get; set; }
            public Decimal ItratRbtNidhet { get; set; }
            public Decimal ItratRibitSiluk { get; set; }

            public int MisparMaslul { get; set; }
            public String MisparTik { get; set; }
            public Decimal RbtMemutzatMetuemet { get; set; }
            public Decimal SachAmlot { get; set; }
            public Decimal SchumBitzua { get; set; }
            public Decimal SchumEhzerAba { get; set; }

            public String ShemMaslul { get; set; }
            public String TaarihHiuvAharon { get; set; }
            public String TaarihHiuvRishon { get; set; }
            public String TeurSugHatzmada { get; set; }
            public String TeurSugRbt { get; set; }
        }
    }
}
