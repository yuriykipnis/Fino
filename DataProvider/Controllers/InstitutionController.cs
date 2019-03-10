using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GoldMountainShared.Dto;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DataProvider.Controllers
{
    [Produces("application/json")]
    [Route("api/institution")]
    public class InstitutionController : Controller
    {
        private readonly IInstitutionRepository _institutionRepository;

        public InstitutionController(IInstitutionRepository institutionRepository)
        {
            _institutionRepository = institutionRepository;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var institutions = await _institutionRepository.GetInstitutions();
            var result = Mapper.Map<IList<InstitutionDto>>(institutions);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var institutions = InitInstitutions();
            var result = Mapper.Map<IList<InstitutionDto>>(institutions);
            return Ok(result);
        }

        private IList<InstitutionDoc> InitInstitutions()
        {
            var institutions = new List<InstitutionDoc>
            {
                new InstitutionDoc
                {
                    Id = Guid.NewGuid(),
                    Name = "Bank Hapoalim",
                    Credentials = new List<string> {"Username", "Password"},
                    Type = InstitutionType.Bank,
                    IsSupported = true,
                },
                new InstitutionDoc
                {
                    Id = Guid.NewGuid(),
                    Name = "Bank Leumi",
                    Credentials = new List<string> {"Username", "Password"},
                    Type = InstitutionType.Bank,
                    IsSupported = false,
                },
                new InstitutionDoc
                {
                    Id = Guid.NewGuid(),
                    Name = "Bank Mizrahi-Tefahot",
                    Credentials = new List<string> {"Username", "Password"},
                    Type = InstitutionType.Bank,
                    IsSupported = true,
                },
                new InstitutionDoc
                {
                    Id = Guid.NewGuid(),
                    Name = "Amex",
                    Credentials = new List<string> {"ID", "Last 6 digits", "Password"},
                    Type = InstitutionType.Credit,
                    IsSupported = true,
                },
                new InstitutionDoc
                {
                    Id = Guid.NewGuid(),
                    Name = "Visa Cal",
                    Credentials = new List<string> {"Username", "Password"},
                    Type = InstitutionType.Credit,
                    IsSupported = true,
                }
            };

            _institutionRepository.RemoveAllInstitutions();
            foreach (var institution in institutions)
            {
                _institutionRepository.AddInstitution(institution);
            }

            return institutions;
        }
    }
}