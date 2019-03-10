using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoldMountainShared.Storage.Documents;
using MaslekaReader.Model;
using MaslekaReader.Model.HeshbonOPolisa;

namespace MaslekaReader
{
    public class MortgageInsurBuilder
    {
        public List<MortgageInsurAccountDoc> CreateAccounts(IEnumerable<Mimshak> data, String userId)
        {
            List<MortgageInsurAccountDoc> accounts = new List<MortgageInsurAccountDoc>();

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

        private List<MortgageInsurAccountDoc> CreateAccounts(String userId, Mimshak item, Mutzar product)
        {
            List<MortgageInsurAccountDoc> accounts = new List<MortgageInsurAccountDoc>();

            if (product.NetuneiMutzar?.SugMutzar == 7 && product?.HeshbonotOPolisot != null)
            {
                foreach (var policy in product.HeshbonotOPolisot.HeshbonOPolisa)
                {
                    var account = CreateAccount(userId, item, product, policy);
                    if (account != null)
                    {
                        accounts.Add(account);
                    }
                }
            }

            return accounts;
        }

        private MortgageInsurAccountDoc CreateAccount(String userId, Mimshak item, Mutzar product, HeshbonOPolisa policy)
        {
            MortgageInsurAccountDoc account;
            try
            {
                var coverages = new List<Coverage>();
                foreach(var cover in policy.Kisuim)
                {
                    foreach (var z in cover.ZihuiKisui)
                    {
                        coverages.Add(new Coverage
                        {
                            CoverageName = z.ShemKisuiYatzran,
                            Amount = z.PirteiKisuiBeMutzar.SchumBituach.GetValueOrDefault(),
                            DueDate = Reader.ConvertStringToDate(z.PirteiKisuiBeMutzar.TaarichTomKisuy),
                            ActualFee = z.PirteiKisuiBeMutzar.DmeiBituahLetashlumBapoal.GetValueOrDefault()
                        });                        
                    }
                }

                account = new MortgageInsurAccountDoc
                {
                    UserId = userId,
                    ProviderName = item.YeshutYatzran?.ShemYatzran,
                    PlanName = policy.ShemTohnit,
                    PolicyId = policy.MisparPolisaOheshbon,
                    PolicyStatus = policy.StatusPolisaOcheshbon == 1 ? PolicyStatus.Active : PolicyStatus.Inactive,
                    DepositFee = policy.PirteiTaktziv.PerutHotzaot.HotzaotBafoalLehodeshDivoach.TotalDmeiNihulHafkada.GetValueOrDefault(),
                    SavingFee = policy.PirteiTaktziv.PerutHotzaot.MivneDmeiNihul.PerutMivneDmeiNihul.FirstOrDefault()?.SheurDmeiNihul.GetValueOrDefault(),
                    Coverage = coverages,

                    WorkDisabilityMonthly = 0,
                    WorkDisabilityOneTime = 0,

                    PolicyOpeningDate = Reader.ConvertStringToDate(policy.TaarichHitztarfutMutzar),
                    ValidationDate = Reader.ConvertStringToDate(policy.TaarichNechonut),
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
