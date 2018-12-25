using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CefScraper.Leumi.Model;
using CefSharp;
using CefSharp.OffScreen;
using Newtonsoft.Json;

namespace CefScraper.Leumi
{
    public class AccountScraper : CommonScraper
    {
        private readonly List<AccountBasic> _accounts = new List<AccountBasic>();

        public AccountScraper(string username, string password, ManualResetEvent resetEvent) 
            : base(username, password, resetEvent)
        {
        }

        public void Start()
        {
            const string loginUrl = "https://hb2.bankleumi.co.il/H/Login.html";
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
                    if (!_accounts.Any())
                    {
                        await GetAccounts();
                    }

                    var selectedAccount = await GetSelectedAccount();
                    await UpdateBalanceForSelectedAccount(selectedAccount);

                    var setAccount = _accounts.FirstOrDefault(a => a.Balance == Decimal.Zero);
                    if (setAccount != null)
                    {
                        await SelectAccount(setAccount, "ddlAccounts_m_ddl");
                        await RefreshAccountView();
                    }
                    else
                    {
                        Logout();
                        StreamOutput();
                    }
                }
            }
        }

        private async Task<AccountBasic> GetSelectedAccount()
        {
            AccountBasic result = null;

            const string script = @"(function(){
                        var select = document.getElementById('ddlAccounts_m_ddl');
                        return select.options[select.selectedIndex].text;
                    })()";

            await Browser.EvaluateScriptAsync(script)
                .ContinueWith(res =>
                {
                    if (!res.IsFaulted && res.Result.Result != null)
                    {
                        var accountNumber = CommonScraper.ToUtf8((String)res.Result.Result);
                        var ac = accountNumber.Split('-', '/');

                        if (ac.Length == 3)
                        {
                            var searchFor = $"{CommonScraper.IntParseSafe(ac[1])}/{CommonScraper.IntParseSafe(ac[2])}";
                            result = _accounts.Find(a => a.AccountNumber.Equals(searchFor));
                        }
                    }
                }, TaskScheduler.Default);

            return result;
        }

        private async Task GetAccounts()
        {
            const string script = @"(function(){
                        var select = document.getElementById('ddlAccounts_m_ddl');
                        var result = [];

                        for(i = 0; i < select.options.length; i++) {
                            if (select.options[i].value != -1){
                                result[i] = select.options[i].text;
                            }
                         }
                        return result;
                    })()";

            await Browser.EvaluateScriptAsync(script)
                .ContinueWith(res =>
                {
                    if (!res.IsFaulted && res.Result.Result != null)
                    {
                        var accountNames = ((List<object>)res.Result.Result).Select(i => i.ToString());
                        foreach (var account in accountNames)
                        {
                            var ac = account.Split('-', '/');
                            _accounts.Add(new AccountBasic
                            {
                                Label = $"{CommonScraper.IntParseSafe(ac[0])} {CommonScraper.IntParseSafe(ac[1])}/{CommonScraper.IntParseSafe(ac[2])}",
                                BranchNumber = CommonScraper.IntParseSafe(ac[0]),
                                AccountNumber = $"{CommonScraper.IntParseSafe(ac[1])}/{CommonScraper.IntParseSafe(ac[2])}"
                            });
                        }
                    }

                }, TaskScheduler.Default);
        }
        
        private void StreamOutput()
        {
            foreach (var account in _accounts)
            {
                string output = JsonConvert.SerializeObject(account);
                Console.WriteLine(output);
            }

            ResetEvent.Set();
        }
    }
}
