using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CefScraper.Leumi.Model;
using CefSharp;
using CefSharp.OffScreen;

namespace CefScraper.Leumi
{
    public class CommonScraper
    {
        protected ChromiumWebBrowser Browser { get; set; }
        protected String Username { get; set; }
        protected String Password { get; set; }
        protected ManualResetEvent ResetEvent { get; set; }

        protected CommonScraper(string username, string password, ManualResetEvent resetEvent)
        {
            Username = username;
            Password = password;
            ResetEvent = resetEvent;

            CefSharpSettings.SubprocessExitIfParentProcessClosed = true;
            CefSharpSettings.ShutdownOnExit = true;

            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var settings = new CefSettings
            {
                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache"),
                LogSeverity = LogSeverity.Error,
                BrowserSubprocessPath = Path.Combine(assemblyFolder, "x86", "CefSharp.BrowserSubprocess.exe")
            };

            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
        }

        protected void GoToCurrent(LoadingStateChangedEventArgs e)
        {
            var dataUrl = "https://hb2.bankleumi.co.il/ebanking/Accounts/ExtendedActivity.aspx#/";
            Browser.Load(dataUrl);
        }

        protected async Task SelectAccount(AccountBasic setAccount)
        {
            string script = @"(function(){
                        var select = document.getElementById('ddlAccounts_m_ddl');
                        for ( var i = 0; i < select.options.length; i++ ) {
                           if (select[i].value != -1 ) {
                                var label = select[i].innerHTML;
                                var l = label.split('-');
                                var ll = l[1].split('/');
                                if (ll[0] == " + setAccount.AccountNumber + @") {
                                    select.value = select[i].value;
                                }
                           }
                        }
                    })()";

            await Browser.EvaluateScriptAsync(script, TaskScheduler.Default);
        }

        protected async Task UpdateBalanceForSelectedAccount(AccountBasic selectedAccount)
        {
            const string script = @"(function(){
                        var balance = document.getElementById('lblBalancesVal').innerHTML;
                        return balance;
                    })()";

            await Browser.EvaluateScriptAsync(script)
                .ContinueWith(res =>
                {
                    if (!res.IsFaulted && res.Result.Result != null)
                    {
                        var balance = CommonScraper.ToUtf8((String)res.Result.Result).TrimStart('₪');
                        selectedAccount.Balance = Convert.ToDecimal(balance);
                    }

                }, TaskScheduler.Default);
        }

        protected async Task RefreshAccountView()
        {
            string script = @"(function(){
                        var button = document.getElementById('btnDisplay');
                        button.click();
                    })()";

            await Browser.EvaluateScriptAsync(script, TaskScheduler.Default);
        }

        protected void Login()
        {
            string script = @"
                        document.getElementById('uid').value = '" + Username + @"';
                        document.getElementById('password').value = '" + Password + @"';
                        document.getElementById('enter').click();
                    ";

            Browser.EvaluateScriptAsync(script);
        }

        protected void Logout()
        {
            const string script = @"(function(){
                        var exitBtn = document.getElementById('LNKLOGOUT_new');
                        exitBtn.click();
                    })()";

            Browser.EvaluateScriptAsync(script, TaskScheduler.Default);
        }

        public static string ToUtf8(string text)
        {
            Encoding wind1252 = Encoding.GetEncoding(1255);
            Encoding utf8 = Encoding.UTF8;

            byte[] wind1252Bytes = wind1252.GetBytes(text);
            byte[] utf8Bytes = Encoding.Convert(wind1252, utf8, wind1252Bytes);
            string utf8String = Encoding.UTF8.GetString(utf8Bytes);

            return utf8String;
        }

        public static int IntParseSafe(string value)
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

        protected async Task TakeSnapshot()
        {
            //Give the browser a little time to render
            Thread.Sleep(500);

            // Wait for the screenshot to be taken.
            var task = Browser.ScreenshotAsync();

            await task.ContinueWith(x =>
            {
                // Make a file to save it to (e.g. C:\Users\jan\Desktop\CefSharp screenshot.png)
                var screenshotPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CefSharp screenshot.png");

                Console.WriteLine("Screenshot ready. Saving to {0}", screenshotPath);

                // Save the Bitmap to the path.
                // The image type is auto-detected via the ".png" extension.
                task.Result.Save(screenshotPath);

                // We no longer need the Bitmap.
                // Dispose it to avoid keeping the memory alive.  Especially important in 32-bit applications.
                task.Result.Dispose();

                // Tell Windows to launch the saved image.
                Process.Start(screenshotPath);
            }, TaskScheduler.Default);
        }
    }
}
