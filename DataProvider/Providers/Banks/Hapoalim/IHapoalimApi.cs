using System;
using System.Collections.Generic;
using DataProvider.Providers.Banks.Hapoalim.Dto;
using DataProvider.Providers.Cards.Cal.Dto;
using DataProvider.Providers.Models;


namespace DataProvider.Providers.Banks.Hapoalim
{
    public interface IHapoalimApi : IDisposable
    {
        IEnumerable<HapoalimAccountResponse> GetAccounts();
        Decimal GetBalance(BankAccount account);
        IEnumerable<HapoalimTransactionResponse> GetTransactions(BankAccount account, DateTime startTime, DateTime endTime);

        HapoalimMortgagesResponse GetMortgages(BankAccount account);
        HapoalimMortgageAssetResponse GetAssetForMortgage(BankAccount account, string loanId);

        HapoalimLoansResponse GetLoans(BankAccount account);
        HapoalimLoanDetailsResponse GetDetailsForLoan(BankAccount account, HapoalimLoansResponse.LoanData loan);
    }
}
