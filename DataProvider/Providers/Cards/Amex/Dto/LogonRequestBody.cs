using Newtonsoft.Json;

namespace DataProvider.Providers.Cards.Amex.Dto
{
    public class LogonBody
    {
        //public string KodMishtamesh { get; set; }

        [JsonProperty(PropertyName = "MisparZihuy")]
        public string MisparZihuy { get; set; }
        
        [JsonProperty(PropertyName = "countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty(PropertyName = "idType")]
        public string IdType { get; set; }

        [JsonProperty(PropertyName = "Sisma")]
        public string Sisma { get; set; }

        [JsonProperty(PropertyName = "cardSuffix")]
        public string CardSuffix { get; set; }
    }
}
