using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.OffScreen;
using LeumiWebScraper.Model;
using Newtonsoft.Json;

namespace CefScraper.Leumi
{
    public class Program
    {
        private static ChromiumWebBrowser _browser;
        private String _username;
        private String _password;
        private List<AccountBasic> _accounts = new List<AccountBasic>();
        private static Program _scraper;
        static ManualResetEvent _resetEvent = new ManualResetEvent(false);

        public static void Main(string[] args)
        {
            _scraper = new Program();
            _scraper.Start(args);
        }

        public void Start(string[] args)
        {
            const string loginUrl = "https://hb2.bankleumi.co.il/H/Login.html";

            _username = args[0];
            _password = args[1];
            var method = args[2];
            
            CefSharpSettings.SubprocessExitIfParentProcessClosed = true;
            CefSharpSettings.ShutdownOnExit = true;

            var settings = new CefSettings
            {
                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache"),
                LogSeverity = LogSeverity.Error
            };

            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
            
            if (method == "accounts")
            {
                _browser = new ChromiumWebBrowser(loginUrl);
                _browser.LoadingStateChanged += LoadCompleted;
            }
            else
            {
                _resetEvent.Set(); // Set
            }

            _resetEvent.WaitOne(); // Blocks until "set"
        }

        private void LoadCompleted(object sender, LoadingStateChangedEventArgs e)
        {
            if (_browser.Address.Contains("eBank_ULI_Login.asp"))
            {
                if (e.IsLoading == false)
                {
                    Login();
                }
            }
            else if (_browser.Address.Contains("ebanking/SO/SPA.aspx#/hpsummary"))
            {
                if (e.IsLoading == false)
                {
                    GoToCurrent(e);
                }
            }
            else if (_browser.Address.Contains("ebanking/Accounts/ExtendedActivity.aspx#/"))
            {
                if (e.IsLoading == false)
                {
                    if (!_accounts.Any())
                    {
                        GetAccounts();
                    }

                    //UpdateBalanceForSelectedAccount();

                    var setAccount = _accounts.FirstOrDefault(a => a.Balance == Decimal.Zero);
                    if (setAccount != null)
                    {
                        //SelectAccount(setAccount);
                    }
                    else
                    {
                        //Logout();
                        //StreamOutput();
                    }
                }
            }
        }

        private void UpdateBalanceForSelectedAccount()
        {
            const string script = @"(function(){
                        var balance = document.getElementById('lblBalancesVal').innerHTML;
                        return balance;
                    })()";

            var selectedAccount = GetSelectedAccount();

            _browser.EvaluateScriptAsync(script)
                .ContinueWith(res =>
                {
                    if (!res.IsFaulted && res.Result.Result != null)
                    {
                       
                    }

                }, TaskScheduler.Default);
        }

        private AccountBasic GetSelectedAccount()
        {
            AccountBasic result = null;

            const string script = @"(function(){
                        var select = document.getElementById('ddlAccounts_m_ddl');
                        return select.options[select.selectedIndex].text;
                    })()";

            _browser.EvaluateScriptAsync(script)
                .ContinueWith(res =>
                {
                    if (!res.IsFaulted && res.Result.Result != null)
                    {
                    }
                }, TaskScheduler.Default);
            return result;
        }

        private void GetAccounts()
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

            _browser.EvaluateScriptAsync(script)
                .ContinueWith(res =>
                {
                    if (!res.IsFaulted && res.Result.Result != null)
                    { 
                        var accountNames = ((List<object>) res.Result.Result).Select(i => i.ToString());
                        foreach (var account in accountNames)
                        {
                            var ac = account.Split('-', '/');
                            _accounts.Add(new AccountBasic
                            {
                                Label = $"{IntParseSafe(ac[0])}-{IntParseSafe(ac[1])}",
                                BranchNumber = IntParseSafe(ac[0]),
                                AccountNumber = ac[1]
                            });
                        }
                        StreamOutput();
                    }

                }, TaskScheduler.Default);
        }

        private void GoToCurrent(LoadingStateChangedEventArgs e)
        {
            var dataUrl = "https://hb2.bankleumi.co.il/ebanking/Accounts/ExtendedActivity.aspx#/";
            _browser.Load(dataUrl);
        }

        private void Login()
        {
            const string script = @"
                        document.getElementById('uid').value = 'MJAMQ2U';
                        document.getElementById('password').value = 'lena1501';
                        document.getElementById('enter').click();
                    ";

            var t = _browser.EvaluateScriptAsync(script);
            //var s = new Snapshot();
            //s.RunTakeScreenshot(t, _browser);
        }

        private void StreamOutput()
        {
            foreach (var account in _accounts)
            {
                string output = JsonConvert.SerializeObject(account);
                Console.WriteLine(output);
                
                //Console.WriteLine($@"{account.Branch}:{account.Account}:{account.Balance}");
            }

            _resetEvent.Set();
        }

        private string ToUtf8(string text)
        {
            Encoding wind1252 = Encoding.GetEncoding(1255);
            Encoding utf8 = Encoding.UTF8;

            byte[] wind1252Bytes = wind1252.GetBytes(text);
            byte[] utf8Bytes = Encoding.Convert(wind1252, utf8, wind1252Bytes);
            string utf8String = Encoding.UTF8.GetString(utf8Bytes);

            return utf8String;
        }

        private static int IntParseSafe(string value)
        {
            int result = 0;
            for (int i = 0; i < value.Length; i++)
            {
                var ch = value[i] - 48;
                if (ch >= 0 && ch <= 9)
                {
                    result = 10 * result + (value[i] - 48);
                }
            }
            return result;
        }
    }
}
