using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProvider.Providers.Interfaces;
using DataProvider.Providers.Models;

namespace DataProvider.Providers.Banks.Tefahot
{
    public class TefahotAccountProvider : IBankAccountProvider
    {
        private readonly ITefahotApi _api;
        private const string ProviderName = "Mizrahi-Tefahot";
        private const int TefahotBankId = 20;

        public TefahotAccountProvider(ITefahotApi api)
        {
            _api = api;
        }

        public IEnumerable<BankAccount> GetAccounts()
        {
            IList<BankAccount> result = new List<BankAccount>();
            var accounts = _api.GetAccounts();

            foreach (var account in accounts)
            {
                result.Add(new BankAccount
                {
                    BankNumber = TefahotBankId,
                    AccountNumber = account.Number,
                    BranchNumber = Convert.ToInt32(account.Branch),
                    Balance = account.Remain,
                    Label = $"{account.Branch} {account.Number}"
                });
            }

            return result;
        }
        
        public BankAccount GetAccount(BankAccountDescriptor accountDescriptor)
        {
            return null;
        }

        public IEnumerable<Transaction> GetTransactions(BankAccountDescriptor accountDescriptor, DateTime startTime, DateTime endTime)
        {
            IList<Transaction> result = new List<Transaction>();
            var transactions = _api.GetTransactions(accountDescriptor.AccountNumber, startTime, endTime);
            foreach (var transaction in transactions)
            {
                transaction.ProviderName = ProviderName;
            }
            return transactions;
        }

        public IEnumerable<Mortgage> GetMortgages(BankAccountDescriptor accountDescriptor)
        {
            IList<Mortgage> result = new List<Mortgage>();
            var mortgages = _api.GetMortgages(accountDescriptor.AccountNumber);
            foreach (var mortgage in mortgages)
            {
                foreach (var loan in mortgage.Maslolim)
                {
                    var sd = loan.TaarihHiuvRishon.Split('-', 'T');
                    var ed = loan.TaarihHiuvAharon.Split('-', 'T');
                    var address = mortgage.KtovetNehes.Split(',');

                    result.Add(new Mortgage
                    {
                        LoanId = $"{loan.MisparTik}/{loan.MisparMaslul}",
                        StartDate = new DateTime(Convert.ToInt32(sd[0]), Convert.ToInt32(sd[1]), Convert.ToInt32(sd[2])),
                        EndDate = new DateTime(Convert.ToInt32(ed[0]), Convert.ToInt32(ed[1]), Convert.ToInt32(ed[2])),
                        OriginalAmount = loan.SchumBitzua,
                        DeptAmount = loan.ItratKrnSiluk,
                        PrepaymentCommission = loan.SachAmlot,
                        InterestRate = loan.AhuzRbtMetuemet,
                        InterestType = loan.TeurSugRbt,
                        LinkageType = loan.TeurSugHatzmada,
                        Asset = new MortgageAsset
                        {
                            CityName  = address[0],
                            StreetName = address[1]
                        }
                    });
                }
            }

            return result;
        }

        public IEnumerable<Loan> GetLoans(BankAccountDescriptor accountDescriptor)
        {
            return new List<Loan>();
        }

       
        public void Dispose()
        {
            _api?.Dispose();
        }

    }
}
