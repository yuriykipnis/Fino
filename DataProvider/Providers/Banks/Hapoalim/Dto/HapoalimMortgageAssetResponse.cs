using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProvider.Providers.Models;

namespace DataProvider.Providers.Banks.Hapoalim.Dto
{
    public class HapoalimMortgageAssetResponse
    {
        public String CityName { get; set; } = String.Empty;
        public String StreetName { get; set; } = String.Empty;
        public String BuildingNumber { get; set; } = String.Empty;

        public IList<MortgageParty> PartyInMortgage { get; set; } = new List<MortgageParty>();

        public class MortgageParty
        {
            public String CityName { get; set; } = String.Empty;
            public String StreetName { get; set; } = String.Empty;
            public String BuildingNumber { get; set; } = String.Empty;

            public String PartyFirstName { get; set; } = String.Empty;
            public String PartyLastName { get; set; } = String.Empty;

            public String PartyRelashionshipCode { get; set; }
        }
    }
}
