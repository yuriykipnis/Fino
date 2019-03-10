using Newtonsoft.Json;

namespace DataProvider.Providers.Cards.Amex.Dto
{
    public class ValidateIdRequestBody
    {
        [JsonProperty(PropertyName = "cardSuffix")]
        public string CardSuffix { get; set; }

        [JsonProperty(PropertyName = "checkLevel")]
        public string CheckLevel { get; set; }

        [JsonProperty(PropertyName = "companyCode")]
        public string CompanyCode { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "idType")]
        public string IdType { get; set; }

        [JsonProperty(PropertyName = "countryCode")]
        public string CountryCode { get; set; }
    }
}
