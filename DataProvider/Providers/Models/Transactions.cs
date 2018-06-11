using System;

namespace DataProvider.Providers.Models
{
    public class Transaction
    {
        public Guid AccountId { get; set; }
        public long Id { get; set; }
        public TransactionType Type { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public double CurrentBalance { get; set; }
        
        public string SupplierId { get; set; }
    }

    public enum TransactionType { Income, Expense };
}
