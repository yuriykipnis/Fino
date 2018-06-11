using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoldMountainShared.Storage.Documents;
using MaslekaReader.Model;
using MaslekaReader.Model.HeshbonOPolisa;

namespace MaslekaReader
{
    public class StudyFundBuilder
    {
        public List<StudyFundAccount> CreateAccounts(IEnumerable<Mimshak> data, String userId)
        {
            List<StudyFundAccount> accounts = new List<StudyFundAccount>();

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

        private List<StudyFundAccount> CreateAccounts(String userId, Mimshak item, Mutzar product)
        {
            List<StudyFundAccount> accounts = new List<StudyFundAccount>();

            if (product.NetuneiMutzar?.SugMutzar == 4 && product?.HeshbonotOPolisot != null)
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

        private StudyFundAccount CreateAccount(String userId, Mimshak item, Mutzar product, HeshbonOPolisa policy)
        {
            StudyFundAccount account;
            try
            {
                var employeerId = policy.PirteiTaktziv.PirteiOved.MprMaasikBeYatzran;
                var employerIdentity = product.NetuneiMutzar.YeshutMaasik.Where(ym => ym.MprMaasikBeYatzran.Equals(employeerId)).FirstOrDefault();

                account = new StudyFundAccount
                {
                    UserId = userId,
                    ProviderName = item.YeshutYatzran?.ShemYatzran,
                    EmployerName = employerIdentity.ShemMaasik,
                    PlanName = policy.ShemTohnit,
                    PolicyId = policy.MisparPolisaOheshbon,
                    PolicyStatus = policy.StatusPolisaOcheshbon == 1 ? PolicyStatus.Active : PolicyStatus.Inactive,
                    TotalSavings = policy.PirteiTaktziv.BlockItrot.Yitrot.YitrotShonot.YitratKaspeyTagmulim.GetValueOrDefault(),
                    WithdrawalDate = Reader.ConvertStringToDate(policy.PirteiTaktziv.BlockItrot.Yitrot.YitrotShonot.MoedNezilutTagmulim),
                    DepositFee = policy.PirteiTaktziv.PerutHotzaot.HotzaotBafoalLehodeshDivoach.TotalDmeiNihulHafkada
                        .GetValueOrDefault(),
                    SavingFee = policy.PirteiTaktziv.PerutHotzaot.MivneDmeiNihul.PerutMivneDmeiNihul.FirstOrDefault(dn => dn.SugHotzaa == 1)?.SheurDmeiNihul.GetValueOrDefault(),
                    YearRevenue = policy.Tsua.SheurTsuaNeto.GetValueOrDefault(),
                    SaverDeposit = policy.PirteiTaktziv.PerutHafrashotLePolisa.FirstOrDefault(phlp => phlp?.SugHafrasha.Value == 8)?.SchumHafrasha.GetValueOrDefault(),
                    EmployerDeposit = policy.PirteiTaktziv.PerutHafrashotLePolisa.FirstOrDefault(phlp => phlp?.SugHafrasha.Value == 9)?.SchumHafrasha.GetValueOrDefault(),
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
