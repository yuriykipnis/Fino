using System;
using System.Collections.Generic;
using GoldMountainShared.Dto.Bank;
using GoldMountainShared.Dto.Credit;

namespace GoldMountainShared.Dto.Provider
{   
    public class ProviderDto
    {
        public String Id { get; set; } = String.Empty;
        public String Name { get; set; } = String.Empty;
        public InstitutionType Type { get; set; }

        public IEnumerable<BankAccountDto> BankAccounts { get; set; } = new List<BankAccountDto>();
        public IEnumerable<CreditCardDto> CreditCards { get; set; } = new List<CreditCardDto>();
    }
}
