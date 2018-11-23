using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace LeumiWebScraper
{
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            //while (!Debugger.IsAttached)
            //{
            //    Thread.Sleep(100);
            //}

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length < 4)
            {
                return;
            }

            var method = args[3];
            
            if (method == "accounts")
            {
                var accountsWindow = new AccountsWindow();
                accountsWindow.Show();
            }

            if (method == "transactions")
            {
                var transactionsWindow = new TransactionsWindow();
                transactionsWindow.Show();    
            }
        }
    }
}
