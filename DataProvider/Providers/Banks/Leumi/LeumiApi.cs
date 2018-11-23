using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Threading.Tasks;
using DataProvider.Providers.Models;
using GoldMountainShared.Storage.Documents;
using Newtonsoft.Json;
using BankAccount = DataProvider.Providers.Models.BankAccount;
using Transaction = DataProvider.Providers.Models.Transaction;
using Mortgage = DataProvider.Providers.Models.Mortgage;
using Loan = DataProvider.Providers.Models.Loan;

namespace DataProvider.Providers.Banks.Leumi
{
    public class LeumiApi : ILeumiApi
    {
        String _accounts;
        String _account;
        private readonly String _userName;
        private readonly String _userPassword;
        private const int LeumiBankId = 10;
        private const string ProviderName = "Leumi";

        public LeumiApi(Provider providerDescriptor)
        {
            if (providerDescriptor == null || providerDescriptor.Credentials.Count == 0)
            {
                throw new ArgumentNullException(nameof(providerDescriptor));
            }

            var crentialValues = providerDescriptor.Credentials.Values.ToArray();
            _userName = crentialValues[0];
            _userPassword = crentialValues[1];
        }

        public IEnumerable<BankAccount> GetAccounts()
        {
            var result = new List<BankAccount>();

            RunScraper("accounts");

            var accounts = _accounts.Split("\r\n");
            foreach (var account in accounts)
            {
                if (!string.IsNullOrEmpty(account))
                {
                    BankAccount newAccount = JsonConvert.DeserializeObject<BankAccount>(account);
                    newAccount.BankNumber = LeumiBankId;
                    result.Add(newAccount);
                }
            }

            return result;
        }

        public IEnumerable<Transaction> GetTransactions(string accountId, DateTime startTime, DateTime endTime)
        {
            var result = new List<Transaction>();
            _account = accountId;

            RunScraper("transactions");

            var accounts = _accounts.Split("\r\n");
            foreach (var account in accounts)
            {
                if (!string.IsNullOrEmpty(account))
                {
                    BankAccount newAccount = JsonConvert.DeserializeObject<BankAccount>(account);
                    result.AddRange(newAccount.Transactions);
                    foreach (var transaction in newAccount.Transactions)
                    {
                        transaction.ProviderName = ProviderName;
                    }
                }
            }

            return result;
        }

        public IEnumerable<Mortgage> GetMortgages(string accountId)
        {
            return new List<Mortgage>();
        }

        public IEnumerable<string> GetBalance(string accountId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Loan> GetLoans(string accountId)
        {
            return new List<Loan>();
        }

        public void Dispose()
        {
        }

        private void RunScraper(string action)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string webScraperPath = Path.Combine(assemblyFolder, "WebScraper", "LeumiWebScraper.exe");

            ProcessStartInfo startInfo = new ProcessStartInfo(webScraperPath);
            startInfo.ArgumentList.Add(_userName);
            startInfo.ArgumentList.Add(_userPassword);
            startInfo.ArgumentList.Add(action);
            if (!String.IsNullOrEmpty(_account))
            {
                startInfo.ArgumentList.Add(_account);
            }

            //startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            //startInfo.UserName = dialog.User;
            //startInfo.Password = dialog.Password;
            
            using (Process process = Process.Start(startInfo))
            {
                _accounts = process?.StandardOutput.ReadToEnd();
                string err = process?.StandardError.ReadToEnd();
                process?.WaitForExit();
            }
        }

        private SecureString GetSecureString(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return null;
            else
            {
                SecureString result = new SecureString();
                foreach (char c in source.ToCharArray())
                    result.AppendChar(c);
                return result;
            }
        }
    }
}
