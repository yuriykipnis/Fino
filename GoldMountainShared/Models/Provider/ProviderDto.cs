using System;
using System.Collections.Generic;
using GoldMountainShared.Models.Bank;
using GoldMountainShared.Models.Credit;

namespace GoldMountainShared.Models.Provider
{   
    public class ProviderDto
    {
        public String Id { get; set; } = String.Empty;
        public String Name { get; set; } = String.Empty;
        public InstitutionType Type { get; set; }

        public IEnumerable<BankAccountDto> BankAccounts { get; set; } = new List<BankAccountDto>();
        public IEnumerable<CreditAccountDto> CreditAccounts { get; set; } = new List<CreditAccountDto>();

    }
}
