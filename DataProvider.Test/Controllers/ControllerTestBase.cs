using System;
using System.Collections.Generic;
using System.Text;
using DataProvider.Controllers;
using DataProvider.Providers.Banks.Leumi.Dto;
using DataProvider.Providers.Cards.Cal.Dto;
using DataProvider.Providers.Mapping;
using DataProvider.Providers.Models;
using DataProvider.Services;
using GoldMountainShared.Dto;
using GoldMountainShared.Dto.Bank;
using GoldMountainShared.Dto.Credit;
using GoldMountainShared.Dto.Provider;
using GoldMountainShared.Dto.Shared;
using GoldMountainShared.Storage;
using GoldMountainShared.Storage.Documents;
using Microsoft.Extensions.Options;

namespace DataProvider.Test.Controllers
{
    public class ControllerTestBase : TestBase
    {
        public new static object MapperInitLock = new object();

        protected OptionsWrapper<DbSettings> _options;

        protected ControllerTestBase()
        {
            InitializeOptions();
        }

        protected void InitializeOptions()
        {
            var dbSettings = new DbSettings
            {
                ConnectionString = "mongodb://admin:abc123!@localhost",
                Database = "DataProviderDbTest"
            };

            _options = new OptionsWrapper<DbSettings>(dbSettings);
        }
    }
}
