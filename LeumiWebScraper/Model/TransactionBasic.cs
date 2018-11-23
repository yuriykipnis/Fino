using System;

namespace LeumiWebScraper.Model
{
    public class TransactionBasic
    {
        public long Id { get; set; }
        public TransactionType Type { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Description { get; set; }
        public Decimal Amount { get; set; }
        public Decimal CurrentBalance { get; set; }
        public string SupplierId { get; set; }
    }

    public enum TransactionType { Income, Expense };
}
