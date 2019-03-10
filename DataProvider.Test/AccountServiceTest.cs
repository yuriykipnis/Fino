using System;
using System.Collections.Generic;
using System.Text;
using DataProvider.Providers;
using DataProvider.Services;
using GoldMountainShared.Storage;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Xunit;

namespace DataProvider.Test
{
    public class AccountServiceTest
    {
        private OptionsWrapper<DbSettings> _options;

        public AccountServiceTest()
        {
            var dbSettings = new DbSettings
            {
                ConnectionString = "mongodb://admin:abc123!@localhost",
                Database = "DataProviderDbTest"
            };

            _options = new OptionsWrapper<DbSettings>(dbSettings);
        }

        [Fact]
        public void AccountServiceTest_Success()
        {
            var accountService = new AccountService(new ProviderFactory(), 
                                                    new BankAccountRepository(_options),
                                                    new CreditCardRepository(_options));

            
        }
    }
}
