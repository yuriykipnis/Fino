using System;
using System.Collections.Generic;
using System.Text;
using GoldMountainShared.Storage.Converters;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace GoldMountainShared.Storage.Documents
{
    public class Loan
    {
        [BsonId]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId InternalId { get; set; }

        public Guid Id { get; set; } = Guid.NewGuid();
        public String LoanId { get; set; } = String.Empty;
        public String UserId { get; set; } = String.Empty;

        public DateTime StartDate { get; set; } = DateTime.MinValue;
        public DateTime EndDate { get; set; } = DateTime.MinValue;
        public DateTime NextPaymentDate { get; set; } = DateTime.MinValue;

        public Double OriginalAmount { get; set; } = 0;
        public Double DeptAmount { get; set; } = 0;
        public Double LastPaymentAmount { get; set; } = 0;
        public Double PrepaymentCommission { get; set; } = 0;

        public String InterestType { get; set; } = String.Empty;
        public String LinkageType { get; set; } = String.Empty;
        public String InsuranceCompany { get; set; } = String.Empty;

        public IList<SubLoan> SubLoans { get; set; } = new List<SubLoan>();
        
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public class SubLoan
        {
            public String Id { get; set; }

            public Double OriginalAmount { get; set; } = 0;
            public Double PrincipalAmount { get; set; } = 0;
            public Double InterestAmount { get; set; } = 0;
            public Double DebtAmount { get; set; } = 0;
            public DateTime NextExitDate { get; set; } = DateTime.MinValue;

            public DateTime StartDate { get; set; } = DateTime.MinValue;
            public DateTime EndDate { get; set; } = DateTime.MinValue;
            public Double InterestRate { get; set; } = 0;
        }
    }
}
