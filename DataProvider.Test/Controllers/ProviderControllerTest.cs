using System;
using System.Collections.Generic;
using System.Linq;
using DataProvider.Controllers;
using DataProvider.Providers;
using DataProvider.Providers.Banks.Leumi.Dto;
using DataProvider.Providers.Cards.Cal.Dto;
using DataProvider.Providers.Models;
using DataProvider.Services;
using GoldMountainShared.Dto;
using GoldMountainShared.Dto.Bank;
using GoldMountainShared.Dto.Credit;
using GoldMountainShared.Dto.Provider;
using GoldMountainShared.Dto.Shared;
using GoldMountainShared.Storage;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Xunit;

namespace DataProvider.Test.Controllers
{
    public class ProviderControllerTest : ControllerTestBase, IDisposable
    {
        private String _userId;
        private ProviderController _controller;

        public ProviderControllerTest()
        {
            InitializeController();
            InitializeUserRepository();
        }

        [Fact]
        public void GetTest_Success()
        {
            InitializeMapper();

            var providerDto = CreateProviderDto();
            var postResult = _controller.Post(providerDto).Result;
            var okPostResult = postResult as OkObjectResult;
            var provider = okPostResult?.Value as ProviderDto;

            var getResult = _controller.Get(provider.Id).Result;
            var okGetResult = getResult as OkObjectResult;
            var gotProvider = okGetResult?.Value as ProviderDto; ;

            Assert.NotNull(gotProvider);
            Assert.True(gotProvider.Id == provider.Id);
            Assert.NotNull(gotProvider.BankAccounts);
            Assert.NotNull(gotProvider.CreditCards);
            Assert.True(gotProvider.Name == "Cal");
            Assert.True(gotProvider.Type == InstitutionType.Credit);
        }

        [Fact]
        public void GetTest_Failed()
        {
            InitializeMapper();

            var getResult = _controller.Get(new Guid().ToString()).Result;
            var notFoundGetResult = getResult as NotFoundResult;

            Assert.NotNull(notFoundGetResult);
        }

        [Fact]
        public void PostTest_Success()
        {
            InitializeMapper();

            var providerDto = CreateProviderDto();
            var result = _controller.Post(providerDto).Result;

            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var provider = okObjectResult.Value as ProviderDto;
            Assert.NotNull(provider);
            Assert.NotEmpty(provider.Id);
            Assert.NotNull(provider.BankAccounts);
            Assert.NotNull(provider.CreditCards);
            Assert.True(provider.Name == "Cal");
            Assert.True(provider.Type == InstitutionType.Credit);
        }

        [Fact]
        public void DeleteTest_Success()
        {
            InitializeMapper();

            var providerDto = CreateProviderDto();

            var postResult = _controller.Post(providerDto).Result;
            var okPostResult = postResult as OkObjectResult;
            var provider = okPostResult?.Value as ProviderDto;

            var deleteResult = _controller.Delete(provider?.Id).Result;
            var okDeleteResult = deleteResult as OkObjectResult;
            var isDeleted = (Boolean)okDeleteResult?.Value;

            Assert.True(isDeleted);
        }

        [Fact]
        public void DeleteTest_Failed()
        {
            InitializeMapper();

            var deleteResult = _controller.Delete(new Guid().ToString()).Result;
            var okDeleteResult = deleteResult as OkObjectResult;
            var isDeleted = (Boolean)okDeleteResult?.Value;

            Assert.False(isDeleted);
        }

        private ProviderCreatingDto CreateProviderDto()
        {
            return new ProviderCreatingDto
            {
                UserId = _userId.ToString(),
                Type = InstitutionType.Credit,
                Name = "Cal",
                Credentials = new Dictionary<String, String>
                {
                    { "username", "YURIYK81" },
                    { "password", "2w3e4r5t" }
                },
                BankAccounts = new List<BankAccountDto>(),
                CreditCards = new List<CreditCardDto>()
            };
        }
        
        private void InitializeUserRepository()
        {
            var usersRepository = new UserRepository(_options);
            var result = usersRepository.AddUser(new UserDoc
            {
                Name = "Tester",
                Email = "tester@gmail.com",
                Accounts = new List<String>()
            });

            _userId = usersRepository.GetAllUsers().Result.FirstOrDefault().Id;
        }

        private void InitializeController()
        {
            var bankAccountRepository = new BankAccountRepository(_options);
            var creditCardRepository = new CreditCardRepository(_options);
            var providerRepository = new ProviderRepository(_options);
            var providerFactory = new ProviderFactory();
            var accountService = new AccountService(new ProviderFactory(), bankAccountRepository, creditCardRepository);

            _controller = new ProviderController(providerRepository, providerFactory, accountService,
                bankAccountRepository, creditCardRepository);
        }

        private void CleanUserRepository()
        {
            var usersRepository = new UserRepository(_options);
            var result = usersRepository.RemoveAllUsers();
        }

        private void CleanProviderRepository()
        {
            var providerRepository = new ProviderRepository(_options);
            providerRepository.RemoveAllProviders();
        }

        public void Dispose()
        {
            CleanUserRepository();
            CleanProviderRepository();
        }
    }
}
