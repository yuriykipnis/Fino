using System;
using System.Collections.Generic;
using GoldMountainShared.Dto.Shared;

namespace GoldMountainShared.Dto.Credit
{
    public class CreditCardDto
    {
        public String Id { get; set; }
        public String ProviderId { get; set; }

        public String LastDigits { get; set; }
        public String CardName { get; set; }
        public String TypeDescription { get; set; } // not mandatory
        public DateTime ExpirationDate { get; set; }
        public String BankAccount { get; set; }
        public int BankBranchNumber { get; set; }
        public String BankName { get; set; }

        public IEnumerable<CreditCardDebitDto> Debits { get; set; } = new List<CreditCardDebitDto>();

        public Boolean IsActive { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.MinValue;
    }

    public class CreditCardDebitDto
    {
        public DateTime Date { get; set; }
        public Decimal Amount { get; set; }
        public IList<CreditCardTransactionDto> Transactions { get; set; } = new List<CreditCardTransactionDto>();

    }

    public class CreditCardTransactionDto
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

        public String DealSector { get; set; }
    }
}
