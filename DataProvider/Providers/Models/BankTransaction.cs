using System;

namespace DataProvider.Providers.Models
{
    public class BankTransaction
    {
        public String Id { get; set; }
        public String AccountId { get; set; }
        
        public DateTime PurchaseDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public Decimal Amount { get; set; }
        public Decimal CurrentBalance { get; set; }

        public TransactionType Type { get; set; }
        public Boolean IsFee { get; set; }

        public string Description { get; set; }
        public string SupplierId { get; set; }

        public String ProviderName { get; set; } = String.Empty;
    }

    public enum TransactionType { Income, Expense, None };
}
