using System;

namespace GoldMountainShared.Dto.Shared
{
    public class TransactionDto
    {
        public String Id { get; set; } = String.Empty;
        public TransactionType Type { get; set; }
        public Boolean IsFee { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.MinValue;
        public DateTime PaymentDate { get; set; } = DateTime.MinValue;
        public String Description { get; set; } = String.Empty;
        public String ProviderName { get; set; } = String.Empty;
        public Decimal Amount { get; set; } = 0;
        public Decimal CurrentBalance { get; set; } = 0;
    }    
}
