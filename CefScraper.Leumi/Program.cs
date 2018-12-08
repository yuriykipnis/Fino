using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.OffScreen;
using Newtonsoft.Json;

namespace CefScraper.Leumi
{
    public class Program
    {

        static readonly ManualResetEvent ResetEvent = new ManualResetEvent(false);
        private static int timeout = 60000;

        public static void Main(string[] args)
        {
            //while (!Debugger.IsAttached)
            //{
            //    Thread.Sleep(100);
            //}

            var username = args[0];
            var password = args[1];
            var method = args[2];

            if (method == "accounts")
            {
                var scraper = new AccountScraper(username, password, ResetEvent);
                scraper.Start();
            }
            else if (method == "transactions")
            {
                var accountNumber = args[3];
                var scraper = new TransactionScraper(username, password, ResetEvent);
                scraper.Start(accountNumber);
            }
            else if (method == "loans")
            {
                ResetEvent.Set();
            }

            ResetEvent.WaitOne(timeout); // Blocks until "set"
        }
    }
}
