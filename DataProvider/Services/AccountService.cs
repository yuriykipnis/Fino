using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProvider.Providers.Interfaces;
using DataProvider.Providers.Models;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using BankAccountDoc = GoldMountainShared.Storage.Documents.BankAccountDoc;

namespace DataProvider.Services
{
    public class AccountService : IAccountService
    {
        private readonly IProviderFactory _providerFactory;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly ICreditCardRepository _creditAccountRepository;
        public static int UpdateInterval = 24;

        public AccountService(IProviderFactory providerFactory, 
                              IBankAccountRepository bankAccountRepository, 
                              ICreditCardRepository creditAccountRepository)
        {
            _providerFactory = providerFactory;
            _bankAccountRepository = bankAccountRepository;
            _creditAccountRepository = creditAccountRepository;
        }
        
        public async Task<IEnumerable<BankAccountDoc>> UpdateBankAccountsForProvider(ProviderDoc provider)
        {
            var result = new List<BankAccountDoc>();
            using (var dataProvider = await _providerFactory.CreateDataProvider(provider))
            {
                var accounts = ((IBankAccountProvider)dataProvider).GetAccounts();
                foreach (var accountId in provider.Accounts)
                {
                    var account = await UpdateBankAccount(accountId, accounts, dataProvider);
                    result.Add(account);
                }
            }

            return result;
        }
        
        public async Task<IEnumerable<CreditCard>> UpdateCreditCards(ProviderDoc provider)
        {
            var result = new List<CreditCard>();

            using (var dataProvider = await _providerFactory.CreateDataProvider(provider))
            {
                foreach (var id in provider.Accounts)
                {
                    var outdatedCardDoc = await _creditAccountRepository.FindCardByCriteria(a => a.Id.Equals(id));
                    var outdatedCard = AutoMapper.Mapper.Map<CreditCard>(outdatedCardDoc);
                    var cardUpdate = FetchCardUpdate(outdatedCardDoc, dataProvider);

                    var card = MergeCardUpdate(outdatedCard, cardUpdate);
                    result.Add(card);
                }
            }

            return result;
        }
        
        private CreditCard FetchCardUpdate(CreditCardDoc outdatedCard, IAccountProvider dataProvider)
        {
            var accountDescriptor = AutoMapper.Mapper.Map<CreditCardDescriptor>(outdatedCard);

            var startTime = outdatedCard.UpdatedOn;
            var endTime = DateTime.Now.AddMonths(1);
            var card = ((ICreditCardProvider)dataProvider)
                .GetCardsWithTransactions(new List<CreditCardDescriptor>{accountDescriptor}, startTime, endTime).FirstOrDefault();

            return card;
        }

        private CreditCard MergeCardUpdate(CreditCard card, CreditCard cardUpdate)
        {
            if (cardUpdate == null) return card;

            foreach (var debit in cardUpdate.Debits)
            {
                var debitsUpdate = card.Debits.Where(d => d.Date.Date.Equals(debit.Date.Date)).ToList();
                if (debitsUpdate.Count == 0) // new debit
                {
                    card.Debits.Add(debit);
                }
                else //updated debit
                {
                    foreach (var debitUpdate in debitsUpdate)
                    {
                        if (SameDebits(debit, debitUpdate))
                        {
                            debit.Amount = debitUpdate.Amount;
                            var transactionsToAdd = debitUpdate.Transactions.Where(t => !IsExisted(t, debit.Transactions));
                            debit.Transactions.ToList().AddRange(transactionsToAdd);
                        }
                    }
                }
            }

            return card;
        }

        private bool SameDebits(CreditCardDebitPeriod debit, CreditCardDebitPeriod debitUpdate)
        {
            return debitUpdate.Transactions.Any(t => !IsExisted(t, debit.Transactions));
        }

        private bool IsExisted(CreditCardTransaction transaction, IList<CreditCardTransaction> transactions)
        {
            return transactions.Any(t => t.Id == transaction.Id);
        }


        private async Task<BankAccountDoc> UpdateBankAccount(String accountId, IEnumerable<BankAccount> accounts, IAccountProvider dataProvider)
        {
            var accountToUpdate = await _bankAccountRepository.FindAccountByCriteria(a => a.Id.Equals(accountId));
            var updatedAccount = GenerateBankAccount(accounts, accountToUpdate);
            var accountDescriptor = AutoMapper.Mapper.Map<BankAccountDescriptor>(accountToUpdate);

            if (updatedAccount == null || accountToUpdate.UpdatedOn.AddDays(1) > DateTime.Now)
            {
                return accountToUpdate;
            }

            var now = DateTime.Now;
            var startOfThisMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var startTime = DateTime.Now.AddYears(-1); ;
            var endTime = now;

            var transactions = ((IBankAccountProvider)dataProvider).GetTransactions(accountDescriptor, startTime, endTime);
            var mortgages = ((IBankAccountProvider)dataProvider).GetMortgages(accountDescriptor);
            var loans = ((IBankAccountProvider)dataProvider).GetLoans(accountDescriptor);

            accountToUpdate.Transactions = AutoMapper.Mapper.Map<IEnumerable<TransactionDoc>>(transactions);
            accountToUpdate.Mortgages = AutoMapper.Mapper.Map<IEnumerable<MortgageDoc>>(mortgages);
            accountToUpdate.Loans = AutoMapper.Mapper.Map<IEnumerable<LoanDoc>>(loans);
            accountToUpdate.Balance = updatedAccount.Balance;

            foreach (var loan in accountToUpdate.Mortgages)
            {
                loan.UserId = accountToUpdate.UserId;
            }

            foreach (var loan in accountToUpdate.Loans)
            {
                loan.UserId = accountToUpdate.UserId;
            }

            return accountToUpdate;
        }

        private static BankAccount GenerateBankAccount(IEnumerable<BankAccount> accounts, BankAccountDoc accountToUpdate)
        {
            var updatedAccount = accounts.FirstOrDefault(a =>
                a.AccountNumber.Equals(accountToUpdate.AccountNumber)
                && a.BankNumber.Equals(accountToUpdate.BankNumber)
                && a.BranchNumber.Equals(accountToUpdate.BranchNumber));
            return updatedAccount;
        }
    }
}
