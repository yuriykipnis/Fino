using System;

namespace GoldMountainShared.Dto.Credit
{
    public class _CreditCardCreatingDto
    {
        public String Name { get; set; } = String.Empty;
        public String Club { get; set; } = String.Empty;
        public String UserName { get; set; } = String.Empty;
        public String CardNumber { get; set; } = String.Empty;
        public DateTime ExpirationDate { get; set; } = DateTime.MaxValue;
        public String BankAccount { get; set; } = String.Empty;
        public String BankName { get; set; } = String.Empty;
    }
}
