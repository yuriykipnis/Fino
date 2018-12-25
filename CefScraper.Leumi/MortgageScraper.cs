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
    public class MortgageScraper : CommonScraper
    {
        private string _accountName;
        private List<String> _mortgageLinks;
        private List<MortgageBasic> _mortgages;

        public MortgageScraper(string username, string password, ManualResetEvent resetEvent) 
            : base(username, password, resetEvent)
        {
            _mortgages = new List<MortgageBasic>();
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
                    GoToMortgages(e);
                }
            }
            else if (Browser.Address.Contains("ebanking/LoanAndMortgages/DisplayLoansAndMortgagesSummary.aspx"))
            {
                if (e.IsLoading == false)
                {
                    _mortgageLinks = await UpdateLinkToMortgages();
                    if (_mortgageLinks.Any())
                    {
                        SelectNextMortgage();
                        return;
                    }

                    Logout();
                    StreamOutput();
                }
            }
            else if (Browser.Address.Contains("ebanking/LoanAndMortgages/DisplayMortgagesActivityNew.aspx"))
            {
                if (e.IsLoading == false)
                {
                    var mortgage = await UpdateMortgage();
                    _mortgages.AddRange(mortgage);

                    if (
                        _mortgageLinks.Any())
                    {
                        SelectNextMortgage();
                        return;
                    }

                    Logout();
                    StreamOutput();
                }
            }
        }

        private void GoToMortgages(LoadingStateChangedEventArgs e)
        {
            var dataUrl = "https://hb2.bankleumi.co.il/ebanking/LoanAndMortgages/DisplayLoansAndMortgagesSummary.aspx#";
            Browser.Load(dataUrl);
        }

        private async Task<List<String>> UpdateLinkToMortgages()
        {
            List<String> result = new List<String>();

            string script = @"(function(){
                        var table = document.getElementById('MortgagesPrivateNIS');
                        var result = [];
                        for (var i = 1; i < table.rows.length-1;  i++) {
                            var row = table.rows[i];
                            var cells = row.cells;
                            result[i-1] = cells[0].childNodes[0].childNodes[0].href;
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

        private void SelectNextMortgage()
        {
            String mortgage = String.Empty;
            if (_mortgageLinks.Any())
            {
                mortgage = _mortgageLinks.FirstOrDefault();
                _mortgageLinks.Remove(mortgage);
            }

            Browser.Load(mortgage);
        }

        private async Task<List<MortgageBasic>> UpdateMortgage()
        {
            var result = new List<MortgageBasic>();
            var splittedAccount = _accountName.Split('/');
            string account = splittedAccount[0] + splittedAccount[1];

            string script = @"(function(){
                        var result = [];                   

                        var table = document.getElementById('tblGeneralInfo');
                        var accountName = table.rows[0].cells[1].innerText;
                        if (!accountName.endsWith(" + account + @")) {
                            return result;
                        }

                        var table = document.getElementById('tblSubLoan');

                        var type;
                        var skipedLines = 1;
                        for (var i = 1, row; row = table.rows[i]; i++) {
                            var cells = row.cells;
                            if (cells.length == 1) {
                                type = cells[0].innerText;
                                skipedLines++;
                                continue;
                            }

                            cells[0].onclick();
                            result[i-skipedLines] = {};
                            result[i-skipedLines].loanId = cells[0].innerText;
                            result[i-skipedLines].originalAmount = cells[1].innerText.slice(2);

                            result[i-skipedLines].interestRate = cells[6].innerText.slice(0, -1);
                            result[i-skipedLines].startDate = cells[4].innerText;
                            result[i-skipedLines].endDate = cells[5].innerText;
                            result[i-skipedLines].interestType = type;

                            var inner_table = table.rows[i+1].getElementsByTagName('table')[0];
                            result[i-skipedLines].lastPaymentAmount = inner_table.rows[0].cells[1].innerText.slice(2);                            
                            result[i-skipedLines].deptAmount = inner_table.rows[1].cells[1].innerText.slice(2);                            
                            result[i-skipedLines].interestAmount = inner_table.rows[2].cells[1].innerText.slice(2);                            

                            if (!isNaN(inner_table.rows[3].cells[1].innerText)) {
                                result[i-skipedLines].prepaymentCommission = inner_table.rows[3].cells[1].innerText.slice(2);                            
                            } else {
                                result[i-skipedLines].prepaymentCommission = '0';                            
                            }

                            result[i-skipedLines].linkageType = inner_table.rows[0].cells[3].innerText;                            
                            result[i-skipedLines].nextExitDate = '01/01/01';

                            cells[0].onclick();
                            i++;
                            skipedLines++;
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
                            var r = JsonConvert.DeserializeObject<List<MortgageBasic>>(res.Result.Result.ToString());
                            result.AddRange(r);
                        }
                        catch (Exception e)
                        {

                        }
                    }

                }, TaskScheduler.Default);

            return result;
        }

        private void StreamOutput()
        {
            string output = JsonConvert.SerializeObject(_mortgages);
            Console.WriteLine(output);

            ResetEvent.Set();
        }
    }
}
