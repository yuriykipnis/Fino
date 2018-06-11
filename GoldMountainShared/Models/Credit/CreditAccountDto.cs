using System;
using System.Collections.Generic;
using System.Text;
using GoldMountainShared.Models.Shared;

namespace GoldMountainShared.Models.Credit
{
    public class CreditAccountDto
    {
        public String Id { get; set; } = String.Empty;
        public String Label { get; set; } = String.Empty;
        public String ProviderName { get; set; } = String.Empty;
        public String Name { get; set; } = String.Empty;
        public String Club { get; set; } = String.Empty;
        public String UserName { get; set; } = String.Empty;
        public String CardNumber { get; set; } = String.Empty;
        public DateTime ExpirationDate { get; set; } = DateTime.MaxValue;
        public String BankAccount { get; set; } = String.Empty;
        public String BankName { get; set; } = String.Empty;

        public IEnumerable<TransactionDto> Transactions { get; set; } = new List<TransactionDto>();

        public Boolean IsActive { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.MinValue;
    }
}
