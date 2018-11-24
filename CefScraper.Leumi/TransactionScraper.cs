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
    public class TransactionScraper : CommonScraper
    {
        private string _accountName;
        private AccountBasic _account;

        public TransactionScraper(string username, string password, ManualResetEvent resetEvent)
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
                    GoToCurrent(e);
                }
            }
            else if (Browser.Address.Contains("ebanking/Accounts/ExtendedActivity.aspx"))
            {
                if (e.IsLoading == false)
                {
                    if (_account == null)
                    {
                        _account = new AccountBasic{ 
                            AccountNumber = _accountName
                        };

                        await SelectAccount(_account);
                        await SelectPeriod();
                        await SelectDates();
                        await RefreshAccountView();

                        return;
                    }

                    await UpdateTransactionsForSelectedAccount(_account);

                    Logout();
                    StreamOutput();
                }
            }
        }

        private async Task UpdateTransactionsForSelectedAccount(AccountBasic selectedAccount)
        {
            string script = @"(function(){
                        var table = document.getElementById('ctlActivityTable');
                        var result = [];
                        for (var i = 1, row; row = table.rows[i]; i++) {
                            var cells = row.cells;
                            result[i-1] = {};
                            result[i-1].paymentDate = cells[1].innerText;
                            result[i-1].description = cells[2].innerText;
                            result[i-1].supplierId = cells[3].innerText;

                            if (cells[4].innerText && cells[4].innerText.replace(/\s/g,'') !== '') {
                                result[i-1].type = 'expense';
                                result[i-1].amount = cells[4].innerText;
                            }
                            else if (cells[5].innerText && cells[5].innerText.replace(/\s/g,'') !== '') {
                                result[i-1].type = 'income';
                                result[i-1].amount = cells[5].innerText;
                            }                           
                            result[i-1].currentBalance = cells[6].innerText;
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
                            var result = JsonConvert.DeserializeObject<List<TransactionBasic>>(res.Result.Result.ToString());
                            selectedAccount.Transactions = result;
                        }
                        catch (Exception e)
                        {
                         
                        }
                    }

                }, TaskScheduler.Default);
        }

        private async Task SelectPeriod()
        {
            string script = @"(function(){
                        var select = document.getElementById('ddlTransactionPeriod');
                        select.value = select[3].value;
                    })()";

            await Browser.EvaluateScriptAsync(script, TaskScheduler.Default);
        }

        private async Task SelectDates()
        {
            var endDate = DateTime.Now;
            var startDate = endDate.AddYears(-2);
            var start = $"{startDate.Day}/{startDate.Month}/{startDate.Year % 100}";
            var end = $"{endDate.Day}/{endDate.Month}/{endDate.Year % 100}";

            string script = @"(function(){
                        var input = document.getElementById('dtFromDate_textBox');
                        input.value = '" + start + @"';

                        input = document.getElementById('dtToDate_textBox');
                        input.value = '" + end + @"';
                    })()";

            await Browser.EvaluateScriptAsync(script, TaskScheduler.Default);
        }

        private void StreamOutput()
        {
            string output = JsonConvert.SerializeObject(_account.Transactions);

            Console.WriteLine(output);

            ResetEvent.Set();
        }
    }
}
