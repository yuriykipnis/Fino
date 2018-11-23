using System;
using System.Collections.Generic;
using DataProvider.Providers.Banks.Hapoalim.Dto;
using DataProvider.Providers.Models;


namespace DataProvider.Providers.Banks.Hapoalim
{
    public interface IHapoalimApi : IDisposable
    {
        IEnumerable<HapoalimAccountResponse> GetAccountsData();
        HapoalimTransactionsResponse GetTransactions(HapoalimAccountResponse account, DateTime startTime, DateTime endTime);
        HapoalimMortgagesResponse GetMortgages(HapoalimAccountResponse account);
        HapoalimMortgageAssetResponse GetAssetForMortgage(HapoalimAccountResponse account, string loanId);
        HapoalimBalanceResponse GetBalance(HapoalimAccountResponse account);
        HapoalimLoansResponse GetLoans(HapoalimAccountResponse account);
        HapoalimLoanDetailsResponse GetDetailsForLoan(HapoalimAccountResponse account, HapoalimLoansResponse.LoanData loan);
    }
}
