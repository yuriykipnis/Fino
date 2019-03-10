using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoldMountainShared.Storage.Documents;
using MaslekaReader.Model;
using MaslekaReader.Model.HeshbonOPolisa;

namespace MaslekaReader
{
    public class SeInsurBuilder
    {
        public List<SeInsurAccountDoc> CreateAccounts(IEnumerable<Mimshak> data, String userId)
        {
            List<SeInsurAccountDoc> accounts = new List<SeInsurAccountDoc>();

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

        private List<SeInsurAccountDoc> CreateAccounts(String userId, Mimshak item, Mutzar product)
        {
            List<SeInsurAccountDoc> accounts = new List<SeInsurAccountDoc>();

            if (product.NetuneiMutzar?.SugMutzar == 1 || product.NetuneiMutzar?.SugMutzar == 5 && product?.HeshbonotOPolisot != null)
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

        private SeInsurAccountDoc CreateAccount(String userId, Mimshak item, Mutzar product, HeshbonOPolisa policy)
        {
            SeInsurAccountDoc account;
            try
            {
                Double totalSchumKitzvatZikna = 0;
                policy.YitraLefiGilPrisha.Kupot.Kupa.ForEach(k =>
                {
                    totalSchumKitzvatZikna += k.SchumKitzvatZikna.GetValueOrDefault();
                });
                
                double? totalSavings = 0;
                double? deathInsuranceAmount = 0;
                policy.Kisuim.ForEach(kisui =>
                {
                    kisui.ZihuiKisui.ForEach(zihui =>
                    {
                        totalSavings = zihui.SchumeiBituahYesodi?.SchumBituahLemavet.GetValueOrDefault();
                        deathInsuranceAmount = zihui.SchumeiBituahYesodi?.TikratGagHatamLemikreMavet.GetValueOrDefault();
                    });
                });

                var employeerId = policy.PirteiTaktziv.PirteiOved.MprMaasikBeYatzran;
                var employerIdentity = product.NetuneiMutzar.YeshutMaasik.Where(ym => ym.MprMaasikBeYatzran.Equals(employeerId)).FirstOrDefault();

                account = new SeInsurAccountDoc
                {
                    UserId = userId,
                    ProviderName = item.YeshutYatzran?.ShemYatzran,
                    EmployerName = employerIdentity?.ShemMaasik,
                    PlanName = policy.ShemTohnit,
                    PolicyId = policy.MisparPolisaOheshbon,
                    PolicyStatus = policy.StatusPolisaOcheshbon == 1 ? PolicyStatus.Active : PolicyStatus.Inactive,
                    TotalSavings = totalSavings,
                    EoyBalance = policy.PirteiTaktziv.PerutYitrotLesofShanaKodemet.YitratSofShana.GetValueOrDefault(),
                    ExpectedRetirementSavingsNoPremium = policy.YitraLefiGilPrisha.TzviratChisachonChazuyaLeloPremiyot.GetValueOrDefault(),
                    MonthlyRetirementPensionNoPremium = policy.YitraLefiGilPrisha.Kupot.Kupa.LastOrDefault()?.SchumKitzvatZikna.GetValueOrDefault(),
                    ExpectedRetirementSavings  = policy.YitraLefiGilPrisha.TzviratChisachonChazuyaLeloPremiyot.GetValueOrDefault(),
                    MonthlyRetirementPension  = totalSchumKitzvatZikna,
                    DepositFee = policy.PirteiTaktziv.PerutMasluleiHashkaa.FirstOrDefault()?.SheurDmeiNihulHafkada.GetValueOrDefault(),
                    SavingFee = policy.PirteiTaktziv.PerutHotzaot.MivneDmeiNihul.PerutMivneDmeiNihul.FirstOrDefault()?.SheurDmeiNihul.GetValueOrDefault(),
                    YearRevenue  = policy.Tsua.SheurTsuaNeto.GetValueOrDefault(),
                    DeathInsuranceMonthlyAmount = 0,
                    DeathInsuranceAmount = deathInsuranceAmount,

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
