using System;
using System.Collections.Generic;
using GoldMountainShared.Storage.Converters;
using Newtonsoft.Json;

namespace DataProvider.Providers.Models
{
    public class CreditCard
    {
        public String Id { get; set; }
        public String LastDigits { get; set; }
        public String CardName { get; set; }
        public String TypeDescription { get; set; }
        public String OwnerFirstName { get; set; }
        public String OwnerLastName { get; set; }
        public String HolderId { get; set; }
        public CreditCardBankAccount CardAccount { get; set; }
        public IList<CreditCardDebitPeriod> Debits { get; set; } = new List<CreditCardDebitPeriod>();
    }

    public class CreditCardBankAccount
    {
        public String Id { get; set; }
        public String AccountNumber { get; set; }
        public int BankBranchNumber { get; set; }
        public int BankCode { get; set; }
        public String BankName { get; set; }
    }

    public class CreditCardDebitPeriod
    {
        public String CardId { get; set; }
        public String CardLastDigits { get; set; }
        public DateTime Date { get; set; }
        public Decimal Amount { get; set; }

        public IList<CreditCardTransaction> Transactions { get; set; } = new List<CreditCardTransaction>();
    }
}
