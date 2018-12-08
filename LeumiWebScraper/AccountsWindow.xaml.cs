using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using LeumiWebScraper.Model;
using mshtml;
using Newtonsoft.Json;

namespace LeumiWebScraper
{
    public partial class AccountsWindow : Window
    {
        private const string DisableScriptError =
            @"function noError() {
                return true;}      
              window.onerror = noError;";

        private readonly String _username;
        private readonly String _password;
        private readonly List<AccountBasic> _accounts = new List<AccountBasic>();
        private DispatcherTimer _dispatcherTimer;
        private const int Timeout = 50;

        public AccountsWindow()
        {
            InitializeComponent();

            string[] args = Environment.GetCommandLineArgs();
            _username = args[1];
            _password = args[2];

            _accountsBrowser.Navigated += Navigated;
            _accountsBrowser.LoadCompleted += LoadCompleted;

            SetScrapingTimer();
        }

        private void SetScrapingTimer()
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += DispatcherTimerTick;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, Timeout);
            _dispatcherTimer.Start();
        }

        private void DispatcherTimerTick(object sender, EventArgs e)
        {
            _dispatcherTimer.Stop();
            _accountsWindow.Close();
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            _accountsBrowser.Navigate("https://hb2.bankleumi.co.il/H/Login.html");
        }

        private void LoadCompleted(object sender, NavigationEventArgs e)
        {
            if (e.Uri.Segments.Contains("eBank_ULI_Login.asp"))
            {
                Login();
            }
            else if (e.Uri.Segments.Contains("PremiumSummaryNew.aspx"))
            {
                if (!_accounts.Any())
                {
                    GetAccounts();
                }

                GetBalanceForSelectedAccount();

                var setAccount = _accounts.FirstOrDefault(a => a.Balance == Decimal.Zero);
                if (setAccount != null)
                {
                    SelectAccount(setAccount);
                }
                else
                {
                    Logout();
                    StreamOutput();
                    _accountsWindow.Close();
                }
            }
        }

        private void GetBalanceForSelectedAccount()
        {
            var account = GetSelectedAccount();

            HTMLDocument dom = (HTMLDocument)_accountsBrowser.Document;
            object balanceObj = dom.getElementById("lblCurrentBalanceVal");
            if (balanceObj is IHTMLSpanElement balanceEl)
            {
                var balance = ((HTMLSpanElement)balanceEl).innerText.Split(' ')[1];
                account.Balance = Convert.ToDecimal(balance);
            }
        }

        private void SelectAccount(AccountBasic account)
        {
            HTMLDocument dom = (HTMLDocument)_accountsBrowser.Document;
            object accountsObj = dom.getElementById("ddlClientNumber_m_ddl");

            if (accountsObj is IHTMLSelectElement accounts)
            {
                foreach (var option in accounts.options)
                {
                    var utf8bytes = Encoding.UTF8.GetBytes(option.text);
                    String ascii = Encoding.ASCII.GetString(utf8bytes);
                    var ac = ascii.TrimStart('?').TrimEnd('?').Split('-', '/');

                    if (ac.Length >= 2 && ac[1] == account.AccountNumber)
                    {
                        option.SetAttribute("selected", "selected");
                        account.BranchNumber = Convert.ToInt32(ac[0]);
                        account.Label = $"{ac[0]}-{ac[1]}";
                    }
                    else
                    {
                        option.RemoveAttribute("selected");
                    }
                }
                ((HTMLSelectElement)accounts).FireEvent("onchange");
            }
        }

        private AccountBasic GetSelectedAccount()
        {
            AccountBasic result = null;
            HTMLDocument dom = (HTMLDocument)_accountsBrowser.Document;
            object accountsObj = dom.getElementById("ddlClientNumber_m_ddl");

            if (accountsObj is IHTMLSelectElement accounts)
            {
                foreach (var option in accounts.options)
                {
                    if (option.selected)
                    {
                        var utf8Bytes = Encoding.UTF8.GetBytes(option.text);
                        String ascii = Encoding.ASCII.GetString(utf8Bytes);
                        var ac = ascii.TrimStart('?').TrimEnd('?').Split('-', '/');

                        if (ac.Length >= 2)
                        {
                            result = _accounts.Find(a => a.AccountNumber.Equals(ac[1]));
                        }
                    }
                }
            }

            return result;
        }

        private IList<AccountBasic> GetAccounts()
        {
            HTMLDocument dom = (HTMLDocument)_accountsBrowser.Document;
            object accountsObj = dom.getElementById("ddlClientNumber_m_ddl");

            if (accountsObj is IHTMLSelectElement accounts)
            {
                foreach (var option in accounts.options)
                {
                    if (option.value != "-1")
                    {
                        var ac = option.text.Split('-', '/');
                        _accounts.Add(new AccountBasic
                        {
                            Label = String.Format("{0}-{1}",ac[0], ac[1]),
                            BranchNumber = IntParseSafe(ac[0]),
                            AccountNumber = ac[1]
                        });
                    }
                }
            }

            return _accounts;
        }

        private void StreamOutput()
        {
            foreach (var account in _accounts)
            {
                string output = JsonConvert.SerializeObject(account);
                Console.WriteLine(output);

                //Console.WriteLine($@"{account.Branch}:{account.Account}:{account.Balance}");
            }
        }

        private void Login()
        {
            HTMLDocument dom = (HTMLDocument)_accountsBrowser.Document;
            object uidObj = dom.getElementById("uid");
            object passwordObj = dom.getElementById("password");

            if (uidObj is IHTMLInputElement uid
                && passwordObj is IHTMLInputElement password)
            {
                uid.value = _username;
                password.value = _password;
            }

            IHTMLElement btn = dom.getElementById("enter");
            btn?.click();
        }

        private void Logout()
        {
            HTMLDocument dom = (HTMLDocument)_accountsBrowser.Document;
            IHTMLElement btn = dom.getElementById("LNKLOGOUT_new");
            btn?.click();
        }

        private void Navigated(object sender, NavigationEventArgs e)
        {
            //Inject error disable script;
            HTMLDocument dom = (HTMLDocument)_accountsBrowser.Document;

            IHTMLScriptElement scriptErrorSuppressed = (IHTMLScriptElement)dom.createElement("SCRIPT");
            scriptErrorSuppressed.type = "text/javascript";
            scriptErrorSuppressed.text = DisableScriptError;
            IHTMLElementCollection nodes = dom.getElementsByTagName("head");
            foreach (IHTMLElement elem in nodes)
            {
                HTMLHeadElement head = (HTMLHeadElement)elem;
                head.appendChild((IHTMLDOMNode)scriptErrorSuppressed);
            }
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
