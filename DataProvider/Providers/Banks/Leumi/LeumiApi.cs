using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using DataProvider.Providers.Banks.Leumi.Dto;
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

        public IEnumerable<LeumiAccountResponse> GetAccounts()
        {
            var result = new List<LeumiAccountResponse>();

            var data = RunScraper("accounts");
            
            //assumes no errors :)

            var accounts = data.Split("\r\n");
            foreach (var account in accounts)
            {
                if (!string.IsNullOrEmpty(account))
                {
                    LeumiAccountResponse newAccount = JsonConvert.DeserializeObject<LeumiAccountResponse>(account);
                    result.Add(newAccount);
                }
            }

            return result;
        }

        public IEnumerable<LeumiTransactionResponse> GetTransactions(string accountId, DateTime startTime, DateTime endTime)
        {
            var result = new List<LeumiTransactionResponse>();
            _account = accountId;

            var data = RunScraper("transactions");

            //assumes no errors :)

            if (!string.IsNullOrEmpty(data))
            {
                result = JsonConvert.DeserializeObject<List<LeumiTransactionResponse>>(data);
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

        private String RunScraper(string action)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string webScraperPath = Path.Combine(assemblyFolder, "WebScraper", "CefScraper.Leumi.exe");

            ProcessStartInfo startInfo = new ProcessStartInfo(webScraperPath);
            startInfo.ArgumentList.Add(_userName);
            startInfo.ArgumentList.Add(_userPassword);
            startInfo.ArgumentList.Add(action);
            if (!String.IsNullOrEmpty(_account))
            {
                startInfo.ArgumentList.Add(_account);
            }

            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            
            using (Process process = Process.Start(startInfo))
            {
                var output = process?.StandardOutput.ReadToEnd();
                string err = process?.StandardError.ReadToEnd();
                process?.WaitForExit();

                return output;
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
