using System;
using System.Collections.Generic;
using GoldMountainApi.Models;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GoldMountainApi.Controllers
{
    [Produces("application/json")]
    [Route("api/system")]
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

        // Call an initialization - api/system/init
        [HttpGet("{setting}")]
        public string Get(string setting)
        {
            if (setting == "init")
            {
                InitAccounts();
                InitProviders();
                InitInstitutions();

                return "Done";
            }

            return "Unknown";
        }

        private void InitInstitutions()
        {
            _institutionRepository.RemoveAllInstitutions();
            _institutionRepository.AddInstitution(new InstitutionDoc
            {
                Id = Guid.NewGuid(),
                Name = "Bank Hapoalim",
                Credentials = new List<string> {"Username", "Password"},
                IsSupported = true,
                UpdatedOn = DateTime.Now,
                CreatedOn = DateTime.Now
            });
            _institutionRepository.AddInstitution(new InstitutionDoc
            {
                Id = Guid.NewGuid(),
                Name = "Bank Leumi",
                IsSupported = false,
                Credentials = new List<string> { "Username", "Password" },
                UpdatedOn = DateTime.Now,
                CreatedOn = DateTime.Now
            });
            _institutionRepository.AddInstitution(new InstitutionDoc
            {
                Id = Guid.NewGuid(),
                Name = "Bank Discount",
                IsSupported = false,
                Credentials = new List<string> { "Username", "Password" },
                UpdatedOn = DateTime.Now,
                CreatedOn = DateTime.Now
            });
            _institutionRepository.AddInstitution(new InstitutionDoc
            {
                Id = Guid.NewGuid(),
                Name = "AMEX",
                IsSupported = true,
                Credentials = new List<string> { "ID", "Last 6 digits", "Password" },
                UpdatedOn = DateTime.Now,
                CreatedOn = DateTime.Now
            });
        }

        private void InitProviders()
        {
            _providerRepository.RemoveAllProviders();
        }

        private void InitAccounts()
        {
            _accountRepository.RemoveAllAccounts();
        }
    }
}