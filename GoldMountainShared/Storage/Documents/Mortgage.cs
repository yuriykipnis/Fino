using System;
using System.Collections.Generic;
using System.Text;
using GoldMountainShared.Storage.Converters;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace GoldMountainShared.Storage.Documents
{
    public class Mortgage
    {
        [BsonId]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId InternalId { get; set; }

        public Guid Id { get; set; } = Guid.NewGuid();
        public String LoanId { get; set; } = String.Empty;
        public String UserId { get; set; } = String.Empty;

        public DateTime StartDate { get; set; } = DateTime.MinValue;
        public DateTime EndDate { get; set; } = DateTime.MinValue;

        public Decimal OriginalAmount { get; set; } = 0;
        public Decimal DeptAmount { get; set; } = 0;
        public Decimal InterestAmount { get; set; } = 0;

        public Decimal LastPaymentAmount { get; set; } = 0;

        public Decimal PrepaymentCommission { get; set; } = 0;
        public DateTime NextExitDate { get; set; } = DateTime.MinValue;

        public Decimal InterestRate { get; set; } = 0;
        public String InterestType { get; set; } = String.Empty;
        public String LinkageType { get; set; } = String.Empty;
        public String InsuranceCompany { get; set; } = String.Empty;

        public MortgageAsset Asset { get; set; }

        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public class MortgageAsset
        {
            public String CityName { get; set; } = String.Empty;
            public String StreetName { get; set; } = String.Empty;
            public String BuildingNumber { get; set; } = String.Empty;
            public String PartyFirstName { get; set; } = String.Empty;
            public String PartyLastName { get; set; } = String.Empty;
        }

        //public class SubLoan
        //{
        //    public String Id { get; set; }

        //    public Decimal OriginalAmount { get; set; } = 0;
        //    public Decimal PrincipalAmount { get; set; } = 0;
        //    public Decimal InterestAmount { get; set; } = 0;
        //    public Decimal DebtAmount { get; set; } = 0;
        //    public DateTime NextExitDate { get; set; } = DateTime.MinValue;

        //    public DateTime StartDate { get; set; } = DateTime.MinValue;
        //    public DateTime EndDate { get; set; } = DateTime.MinValue;
        //    public Decimal InterestRate { get; set; } = 0;
        //}
    }
}
