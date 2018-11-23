using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProvider.Providers.Models
{
    public class MortgageAsset
    {
        public String CityName { get; set; } = String.Empty;
        public String StreetName { get; set; } = String.Empty;
        public String BuildingNumber { get; set; } = String.Empty;

        public String PartyFirstName { get; set; } = String.Empty;
        public String PartyLastName { get; set; } = String.Empty;
    }
}
