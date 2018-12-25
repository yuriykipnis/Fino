using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CefScraper.Leumi.Model;
using CefSharp;
using CefSharp.OffScreen;
using Newtonsoft.Json;

namespace CefScraper.Leumi
{
    public class LoanScraper : CommonScraper 
    {
        private string _accountName;
        private AccountBasic _account;
        private IList<String> _loans;

        public LoanScraper(string username, string password, ManualResetEvent resetEvent) 
            : base(username, password, resetEvent)
        {
        }

        public void Start(string accountName)
        {
            const string loginUrl = "https://hb2.bankleumi.co.il/H/Login.html";
            _accountName = accountName;

            Browser = new ChromiumWebBrowser(loginUrl);

            Browser.LoadingStateChanged += LoadCompleted;
        }

        private async void LoadCompleted(object sender, LoadingStateChangedEventArgs e)
        {
            if (Browser.Address.Contains("eBank_ULI_Login.asp"))
            {
                if (e.IsLoading == false)
                {
                    Login();
                }
            }
            else if (Browser.Address.Contains("ebanking/SO/SPA.aspx"))
            {
                if (e.IsLoading == false)
                {
                    GoToLoans(e);
                }
            }
            else if (Browser.Address.Contains("ebanking/LoanAndMortgages/DisplayLoansAndMortgagesSummary.aspx"))
            {
                if (e.IsLoading == false)
                {
                    if (_account == null)
                    {
                        _account = new AccountBasic
                        {
                            AccountNumber = _accountName
                        };
                        await SelectAccount(_account, "ddlClientNumber_m_ddl");
                        await RefreshAccountView();
                        Thread.Sleep(500);

                        _loans = await UpdateLinkToLoansForSelectedAccount(_account);
                        if (_loans.Any())
                        {
                            SelectNextLoan();
                            return;
                        }

                        Logout();
                        StreamOutput();
                    }
                }
            }
            else if (Browser.Address.Contains("ebanking/LoanAndMortgages/DisplayLoanActivity.aspx"))
            {
                if (e.IsLoading == false)
                {
                    await UpdateAccountByLoan();

                    if (_loans.Any())
                    {
                        SelectNextLoan();
                        return;
                    }

                    Logout();
                    StreamOutput();
                }
            }
        }

        private void GoToLoans(LoadingStateChangedEventArgs e)
        {
            var dataUrl = "https://hb2.bankleumi.co.il/ebanking/LoanAndMortgages/DisplayLoansAndMortgagesSummary.aspx#";
            Browser.Load(dataUrl);
        }

        private async Task<List<String>> UpdateLinkToLoansForSelectedAccount(AccountBasic selectedAccount)
        {
            List<String> result = new List<String>();

            string script = @"(function(){
                        var table = document.getElementById('TableNIS1');
                        var result = [];
                        for (var i = 1; i < table.rows.length-1;  i++) {
                            var row = table.rows[i];
                            var cells = row.cells;
                            result[i-1] = cells[0].childNodes[1].href;
                        }

                        return JSON.stringify(result);
                    })()";

            await Browser.EvaluateScriptAsync(script)
                .ContinueWith(res =>
                {

                    if (!res.IsFaulted && res.Result.Result != null)
                    {
                        try
                        {
                            result = JsonConvert.DeserializeObject<List<String>>(res.Result.Result.ToString());
                        }
                        catch (Exception e)
                        {

                        }
                    }

                }, TaskScheduler.Default);

            return result;
        }

        private void SelectNextLoan()
        {
            String loan = String.Empty;
            if (_loans.Any())
            {
                loan = _loans.FirstOrDefault();
                _loans.Remove(loan);
            }

            Browser.Load(loan);
        }

        private async Task UpdateAccountByLoan()
        {
            string script = @"(function(){
                        var result = {};                        
                        var table = document.getElementById('pnlVariousData').childNodes[1];
                        result.loanId = table.rows[4].cells[1].innerText;

                        table = document.getElementById('pnlSummaries').childNodes[1];
                        result.originalAmount = table.rows[2].cells[1].innerText;
                        result.deptAmount = table.rows[3].cells[1].innerText;
                        result.nextPayment = table.rows[4].cells[1].innerText;
                        
                        table = document.getElementById('pnlInterestRate').childNodes[1];
                        result.type = table.rows[1].cells[1].innerText;
                        result.interestRate = table.rows[2].cells[1].innerText.slice(0, -1);

                        table = document.getElementById('pnlDates').childNodes[1];
                        result.startDate = table.rows[1].cells[1].innerText;
                        result.endDate = table.rows[2].cells[1].innerText;
                        result.nextPaymentDate = table.rows[3].cells[1].innerText;

                        return JSON.stringify(result);
                    })()";

            await Browser.EvaluateScriptAsync(script)
                .ContinueWith(res =>
                {
                    if (!res.IsFaulted && res.Result.Result != null)
                    {
                        try
                        {
                            var result = JsonConvert.DeserializeObject<LoanBasic>(res.Result.Result.ToString());
                            _account.Loans.Add(result);
                        }
                        catch (Exception e)
                        {

                        }
                    }

                }, TaskScheduler.Default);

        }

        private void StreamOutput()
        {
            string output = JsonConvert.SerializeObject(_account.Loans);

            Console.WriteLine(output);

            ResetEvent.Set();
        }
    }
}
