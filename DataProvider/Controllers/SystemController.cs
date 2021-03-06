﻿using System;
using System.Collections.Generic;
using GoldMountainShared.Dto;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DataProvider.Controllers
{
    [Produces("application/json")]
    [Route("api/System")]
    public class SystemController : Controller
    {
        private readonly IBankAccountRepository _accountRepository;
        private readonly IProviderRepository _providerRepository;
        private readonly IInstitutionRepository _institutionRepository;

        public SystemController(IBankAccountRepository accountRepository, IProviderRepository providerRepository,
                                IInstitutionRepository institutionRepository)
        {
            _accountRepository = accountRepository;
            _providerRepository = providerRepository;
            _institutionRepository = institutionRepository;
        }

        [HttpGet("{setting}")]
        public IActionResult Get(string setting)
        {
            if (setting.Equals("init"))
            {
                InitInstitutions();
                InitProviders();
                InitAccounts();
                
                return Ok();
            }
            
            return StatusCode(500);
        }

        private void InitInstitutions()
        {
            _institutionRepository.RemoveAllInstitutions();

            _institutionRepository.AddInstitution(new InstitutionDoc
            {
                Id = Guid.NewGuid(),
                Name = "Bank Hapoalim",
                Credentials = new List<string> { "Username", "Password" },
                Type = InstitutionType.Bank,
                IsSupported = true,
            });

            _institutionRepository.AddInstitution(new InstitutionDoc
            {
                Id = Guid.NewGuid(),
                Name = "Bank Leumi",
                Credentials = new List<string> { "Username", "Password" },
                Type = InstitutionType.Bank,
                IsSupported = false,
            });

            _institutionRepository.AddInstitution(new InstitutionDoc
            {
                Id = Guid.NewGuid(),
                Name = "Bank Mizrahi-Tefahot",
                Credentials = new List<string> { "Username", "Password" },
                Type = InstitutionType.Bank,
                IsSupported = true,
            });

            _institutionRepository.AddInstitution(new InstitutionDoc
            {
                Id = Guid.NewGuid(),
                Name = "Amex",
                Credentials = new List<string> { "ID", "Last 6 digits", "Password" },
                Type = InstitutionType.Credit,
                IsSupported = true,
            });

            _institutionRepository.AddInstitution(new InstitutionDoc
            {
                Id = Guid.NewGuid(),
                Name = "Visa Cal",
                Credentials = new List<string> { "Username", "Password" },
                Type = InstitutionType.Credit,
                IsSupported = true,
            });
        }

        private void InitProviders()
        {
            _providerRepository.RemoveAllProviders();

            //_providerRepository.AddProvider(new Provider()
            //{
            //    Id = _bankProvider,
            //    Name = "Bank Hapoalim",
            //    Accounts = new List<Guid>
            //    {
            //        _bankAccount1,
            //        _bankAccount2
            //    },
            //    Credentials = new Dictionary<string, string>
            //    {
            //        {"username", "vm61537"},
            //        { "password", "0p9o8i7u"}
            //    },
            //    CreatedOn = DateTime.Now,
            //    UpdatedOn = DateTime.Now
            //});
        }

        private void InitAccounts()
        {
            _accountRepository.RemoveAllAccounts();

            //_accountRepository.AddAccount(new Account()
            //{
            //    Id = _bankAccount1,
            //    Name = "129249",
            //    ProviderId = _bankProvider,
            //    CreatedOn = DateTime.Now,
            //    UpdatedOn = DateTime.Now
            //});
        }
    }
}