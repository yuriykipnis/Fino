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

    public partial class TransactionsWindow : Window
    {
        private const string DisableScriptError =
            @"function noError() {
                return true;}      
              window.onerror = noError;";

        private readonly String _username;
        private readonly String _password;
        private readonly String _account;
        private readonly List<AccountBasic> _accounts = new List<AccountBasic>();
        private DispatcherTimer _dispatcherTimer;
        private const int Timeout = 80;

        public TransactionsWindow()
        {
            InitializeComponent();

            string[] args = Environment.GetCommandLineArgs();
            _username = args[1];
            _password = args[2];
            _account = args[4];

            _transactionsBrowser.Navigated += Navigated;
            _transactionsBrowser.LoadCompleted += LoadCompleted;

            SetScrapingTimer();
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            _transactionsBrowser.Navigate("https://hb2.bankleumi.co.il/H/Login.html");
        }

        private void LoadCompleted(object sender, NavigationEventArgs e)
        {
            if (e.Uri.Segments.Contains("eBank_ULI_Login.asp"))
            {
                Login();
            }
            else if (e.Uri.Segments.Contains("PremiumSummaryNew.aspx"))
            {
                GoToCurrent();
            }
            else if (e.Uri.Segments.Contains("ExtendedActivity.aspx"))
            {
                if (!_accounts.Any())
                {
                    _accounts.Add(new AccountBasic
                    {
                        AccountNumber = _account
                    });

                    SetupDates();
                    SelectAccount(_accounts.FirstOrDefault());
                    return;
                }

                GetBalanceForSelectedAccount();
                GetTransactionsForSelectedAccount();
                Logout();

                StreamOutput();
                _transactionsWindow.Close();
            }
        }

        private void GetTransactionsForSelectedAccount()
        {
            HTMLDocument dom = (HTMLDocument)_transactionsBrowser.Document;
            object ttObj = dom.getElementById("ctlActivityTable");

            var account = GetSelectedAccount();

            if (ttObj is IHTMLTable transactionsTable)
            {
                foreach (var rowObj in transactionsTable.rows)
                {
                    var row = (IHTMLTableRow)rowObj;
                    if (row.rowIndex == 0)
                    {
                        continue;
                    }

                    var transaction = new TransactionBasic();
                    foreach (var cellObj in row.cells)
                    {
                        var cell = (HTMLTableCell)cellObj;
                        switch (cell.cellIndex)
                        {
                            case 1:
                                var date = cell.innerText.Split('/');
                                transaction.PaymentDate = new DateTime(2000+Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0]));
                                break;
                            case 2:
                                Encoding wind1252 = Encoding.GetEncoding(1255);
                                Encoding utf8 = Encoding.UTF8;

                                byte[] wind1252Bytes = wind1252.GetBytes(cell.innerText);
                                byte[] utf8Bytes = Encoding.Convert(wind1252, utf8, wind1252Bytes);
                                string utf8String = Encoding.UTF8.GetString(utf8Bytes);
                                transaction.Description = utf8String;
                                break;
                            case 3:
                                transaction.SupplierId = cell.innerText;
                                break;
                            case 4:
                                if (!String.IsNullOrWhiteSpace(cell.innerText))
                                {
                                    transaction.Type = TransactionType.Expense;
                                    transaction.Amount = Convert.ToDecimal(cell.innerText);
                                }
                                break;
                            case 5:
                                if (!String.IsNullOrWhiteSpace(cell.innerText))
                                {
                                    transaction.Type = TransactionType.Income;
                                    transaction.Amount = Convert.ToDecimal(cell.innerText);
                                }
                                break;
                            case 6:
                                transaction.CurrentBalance = Convert.ToDecimal(cell.innerText);
                                break;
                            default: break;
                        }
                    }

                    account.Transactions.Add(transaction);
                }
            }
        }
       
        private void SetupDates()
        {
            HTMLDocument dom = (HTMLDocument)_transactionsBrowser.Document;
            object accountsObj = dom.getElementById("ddlTransactionPeriod");

            if (accountsObj is IHTMLSelectElement accounts)
            {
                foreach (var option in accounts.options)
                {
                    if (option.value.Equals("004"))
                    {
                        option.SetAttribute("selected", "selected");
                    }
                    else
                    {
                        option.RemoveAttribute("selected");
                    }
                }
                ((HTMLSelectElement)accounts).FireEvent("onchange");
            }

            var now = DateTime.Now;

            object fromDateObj = dom.getElementById("dtFromDate_textBox");
            if (fromDateObj is IHTMLInputElement fromDate)
            {
                var startDate = now.AddYears(-2);
                fromDate.value = String.Format("{0}/{1}/{2}", startDate.Day, startDate.Month, startDate.Year % 100);
            }

            object toDateObj = dom.getElementById("dtToDate_textBox");
            if (toDateObj is IHTMLInputElement toDate)
            {
                toDate.value = String.Format("{0}/{1}/{2}", now.Day, now.Month, now.Year % 100);
            }

            IHTMLElement btn = dom.getElementById("btnDisplayDates");
            btn?.click();
        }

        private void GoToCurrent()
        {
            HTMLDocument dom = (HTMLDocument)_transactionsBrowser.Document;
            object currentAObj = dom.getElementById("navTopImgID_1").parentElement;

            if (currentAObj is IHTMLAnchorElement currentAncor)
            {
                ((HTMLAnchorElement)currentAncor).click();
            }
        }

        private void GetBalanceForSelectedAccount()
        {
            var account = GetSelectedAccount();

            HTMLDocument dom = (HTMLDocument)_transactionsBrowser.Document;
            object balanceObj = dom.getElementById("lblBalancesVal");
            if (balanceObj is IHTMLSpanElement balanceEl)
            {
                var balance = ((HTMLSpanElement)balanceEl).innerText.Split('₪')[1];
                account.Balance = Convert.ToDecimal(balance);
            }
        }

        private void SelectAccount(AccountBasic account)
        {
            HTMLDocument dom = (HTMLDocument)_transactionsBrowser.Document;
            object accountsObj = dom.getElementById("ddlAccounts_m_ddl");

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

                IHTMLElement btn = dom.getElementById("btnDisplayDates");
                btn?.click();
            }
        }

        private AccountBasic GetSelectedAccount()
        {
            AccountBasic result = null;
            HTMLDocument dom = (HTMLDocument)_transactionsBrowser.Document;
            object accountsObj = dom.getElementById("ddlAccounts_m_ddl");

            if (accountsObj is IHTMLSelectElement accounts)
            {
                foreach (var option in accounts.options)
                {
                    if (option.selected)
                    {
                        var utf8Bytes = Encoding.UTF8.GetBytes(option.text);
                        String ascii = Encoding.ASCII.GetString(utf8Bytes);
                        var ac = ascii.TrimStart('?').TrimEnd('?').Split('-','/');

                        if (ac.Length >= 2) { 
                            result = _accounts.Find(a => a.AccountNumber.Equals(ac[1]));
                        }
                    }
                }
            }

            return result;
        }

        private void StreamOutput()
        {
            foreach (var account in _accounts)
            {
                String output = JsonConvert.SerializeObject(account);
                Console.WriteLine(output);
            }
        }

        private void Login()
        {
            HTMLDocument dom = (HTMLDocument)_transactionsBrowser.Document;
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
            HTMLDocument dom = (HTMLDocument)_transactionsBrowser.Document;
            IHTMLElement btn = dom.getElementById("LNKLOGOUT_new");
            btn?.click();
        }

        private void Navigated(object sender, NavigationEventArgs e)
        {
            //Inject error disable script;
            HTMLDocument dom = (HTMLDocument)_transactionsBrowser.Document;

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
            _transactionsWindow.Close();
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
