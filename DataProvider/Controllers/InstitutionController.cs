using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GoldMountainShared.Models;
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
            var result =  Mapper.Map<IEnumerable<InstitutionDto>>(institutions);
            return Ok(result);
        }
    }
}