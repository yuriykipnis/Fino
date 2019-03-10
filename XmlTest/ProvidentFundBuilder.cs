using System;
using System.Collections.Generic;
using System.Text;
using GoldMountainShared.Storage.Documents;
using MaslekaReader.Model;
using MaslekaReader.Model.HeshbonOPolisa;

namespace MaslekaReader
{
    public class ProvidentFundBuilder
    {
        public List<ProvidentFundAccountDoc> CreateAccounts(IEnumerable<Mimshak> data, String userId)
        {
            List<ProvidentFundAccountDoc> accounts = new List<ProvidentFundAccountDoc>();

            foreach (var item in data)
            {
                var products = item.YeshutYatzran?.Mutzarim?.Mutzar;
                if (products != null)
                {
                    foreach (var product in products)
                    {
                        accounts.AddRange(CreateAccounts(userId, item, product));
                    }
                }
            }

            return accounts;
        }

        private List<ProvidentFundAccountDoc> CreateAccounts(String userId, Mimshak item, Mutzar product)
        {
            List<ProvidentFundAccountDoc> accounts = new List<ProvidentFundAccountDoc>();

            if (product.NetuneiMutzar?.SugMutzar == 6 && product?.HeshbonotOPolisot != null)
            {
                foreach (var policy in product.HeshbonotOPolisot.HeshbonOPolisa)
                {
                    var account = CreateAccount(userId, item, policy);
                    if (account != null)
                    {
                        accounts.Add(account);
                    }
                }
            }

            return accounts;
        }

        private ProvidentFundAccountDoc CreateAccount(String userId, Mimshak item, HeshbonOPolisa policy)
        {
            ProvidentFundAccountDoc account;
            try
            {
                account = new ProvidentFundAccountDoc
                {
                    UserId = userId,
                    ProviderName = item.YeshutYatzran?.ShemYatzran,
                    PolicyId = policy.MisparPolisaOheshbon,

                    PolicyStatus = policy.StatusPolisaOcheshbon == 1 ? PolicyStatus.Active : PolicyStatus.Inactive,

                    PolicyOpeningDate = Reader.ConvertStringToDate(policy.TaarichHitztarfutMutzar),
                    ValidationDate = Reader.ConvertStringToDate(policy.TaarichNechonut)
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return account;
        }
    }
}
