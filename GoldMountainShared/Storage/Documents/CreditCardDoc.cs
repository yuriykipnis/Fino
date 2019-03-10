using System;
using System.Collections.Generic;
using GoldMountainShared.Storage.Converters;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace GoldMountainShared.Storage.Documents
{
    public class CreditCardDoc
    {
        public String Id { get; set; }
        public String UserId { get; set; }
        public String ProviderId { get; set; }
        public String ProviderName { get; set; }

        public String LastDigits { get; set; }
        public String CardName { get; set; }
        public String TypeDescription { get; set; }
        public String OwnerFirstName { get; set; }
        public String OwnerLastName { get; set; }
        public String HolderId { get; set; }
        public DateTime ExpirationDate { get; set; } = DateTime.MaxValue;
        public CreditCardBankAccountDoc CardAccount { get; set; }
        public IList<CreditCardDebitPeriodDoc> Debits { get; set; }
        
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }

    public class CreditCardBankAccountDoc
    {
        public String Id { get; set; } 
        public String AccountNumber { get; set; } 
        public int BankBranchNumber { get; set; }
        public int BankCode { get; set; }
        public String BankName { get; set; }
    }

    public class CreditCardDebitPeriodDoc
    {
        public String CardId { get; set; }
        public String CardLastDigits { get; set; }
        public DateTime Date { get; set; }
        public Decimal Amount { get; set; }

        public IList<CreditCardTransactionDoc> Transactions { get; set; } = new List<CreditCardTransactionDoc>();
    }

    public class CreditCardTransactionDoc
    {
        public String Id { get; set; }

        public Decimal PaymentAmount { get; set; }
        public String PaymentCurrency { get; set; }
        public DateTime PaymentDate { get; set; }

        public Decimal DealAmount { get; set; }
        public String DealCurrency { get; set; }
        public DateTime DealDate { get; set; }

        public String Description { get; set; }
        public String Notes { get; set; }

        public String SupplierAddress { get; set; }
        public String DealSector { get; set; }
    }
}
