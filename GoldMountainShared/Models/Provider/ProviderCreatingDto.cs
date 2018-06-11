using System;
using System.Collections.Generic;
using GoldMountainShared.Models.Bank;
using GoldMountainShared.Models.Credit;

namespace GoldMountainShared.Models.Provider
{
    public class ProviderCreatingDto
    {
        public String UserId { get; set; } = String.Empty;
        public String Name { get; set; } = String.Empty;
        public InstitutionType Type { get; set; }

        public IDictionary<String, String> Credentials { get; set; } = new Dictionary<string, string>();

        public IEnumerable<BankAccountCreatingDto> BankAccounts { get; set; } = new List<BankAccountCreatingDto>();
        public IEnumerable<CreditAccountCreatingDto> CreditAccounts { get; set; } = new List<CreditAccountCreatingDto>();
    }
}
