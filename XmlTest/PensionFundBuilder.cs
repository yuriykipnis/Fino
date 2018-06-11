using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoldMountainShared.Storage.Documents;
using MaslekaReader.Model;
using MaslekaReader.Model.HeshbonOPolisa;

namespace MaslekaReader
{
    public class PensionFundBuilder
    {
        public List<PensionFundAccount> CreateAccounts(IEnumerable<Mimshak> data, String userId)
        {
            List<PensionFundAccount> accounts = new List<PensionFundAccount>();

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

        private List<PensionFundAccount> CreateAccounts(String userId, Mimshak item, Mutzar product)
        {
            List<PensionFundAccount> accounts = new List<PensionFundAccount>();

            if (product.NetuneiMutzar?.SugMutzar == 2 && product?.HeshbonotOPolisot != null)
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

        private PensionFundAccount CreateAccount(String userId, Mimshak item, Mutzar product, HeshbonOPolisa policy)
        {
            PensionFundAccount account;
            try
            {
                Double totalSavings = 0;
                policy.PirteiTaktziv.PerutMasluleiHashkaa.ForEach(mh =>
                {
                    totalSavings += mh.SchumTzviraBamaslul.GetValueOrDefault();
                });

                Double totalSchumKitzvatZikna = 0;
                policy.YitraLefiGilPrisha.Kupot.Kupa.ForEach(k =>
                {
                    totalSchumKitzvatZikna += k.SchumKitzvatZikna.GetValueOrDefault();
                });

                var employeerId = policy.PirteiTaktziv.PirteiOved.MprMaasikBeYatzran;
                var employerIdentity = product.NetuneiMutzar.YeshutMaasik.Where(ym => ym.MprMaasikBeYatzran.Equals(employeerId)).FirstOrDefault();
                
                account = new PensionFundAccount
                {
                    UserId = userId,
                    ProviderName = item.YeshutYatzran?.ShemYatzran,
                    EmployerName = employerIdentity.ShemMaasik,
                    PlanName = policy.ShemTohnit,
                    PolicyId = policy.MisparPolisaOheshbon,
                    PolicyStatus = policy.StatusPolisaOcheshbon == 1 ? PolicyStatus.Active : PolicyStatus.Inactive,
                    TotalSavings = totalSavings,
                    ExpectedRetirementSavingsNoPremium = policy.YitraLefiGilPrisha.TzviratChisachonChazuyaLeloPremiyot.GetValueOrDefault(),
                    MonthlyRetirementPensionNoPremium = policy.YitraLefiGilPrisha.Kupot.Kupa.LastOrDefault()?.SchumKitzvatZikna.GetValueOrDefault(),
                    ExpectedRetirementSavings = policy.YitraLefiGilPrisha.TzviratChisachonChazuyaLeloPremiyot.GetValueOrDefault(),
                    MonthlyRetirementPension = totalSchumKitzvatZikna,
                    DepositFee = policy.PirteiTaktziv.PerutHotzaot.MivneDmeiNihul.PerutMivneDmeiNihul.FirstOrDefault(dn => dn.SugHotzaa == 2)?.SheurDmeiNihul.GetValueOrDefault(),
                    SavingFee = policy.PirteiTaktziv.PerutHotzaot.MivneDmeiNihul.PerutMivneDmeiNihul.FirstOrDefault(dn => dn.SugHotzaa == 1)?.SheurDmeiNihul.GetValueOrDefault(),
                    YearRevenue = policy.Tsua.SheurTsuaNeto.GetValueOrDefault(),

                    SaverDeposit = 0,
                    EmployerDeposit = 0,
                    PartnerSurvivors = 0, //policy.NetuneiSheerim.Sheer.FirstOrDefault(s => s.SugZika == 2)?.,
                    ChildrenSurvivors = 0,
                    ParentSurvivors = 0,
                    InvalidPension = 0,
                    WorkDisabilityMonthly = 0,
                    WorkDisabilityOneTime = 0,

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
