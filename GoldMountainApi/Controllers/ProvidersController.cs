using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using GoldMountainApi.Models;
using GoldMountainShared.Dto.Provider;
using GoldMountainShared.Storage.Documents;
using GoldMountainShared.Storage.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoldMountainApi.Controllers
{
    [Produces("application/json")]
    [Route("api/providers")]
    [Authorize]
    public class ProvidersController : Controller
    {

        private readonly IProviderRepository _providerRepository;

        public ProvidersController(IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProviderDto newProvider)
        {
            if (newProvider == null)
            {
                throw new Exception("Something went wrong...");
            }

            var id = Guid.NewGuid();
            try
            {
                
            }
            catch (Exception e)
            {
                throw;
            }

            return Ok(id.ToString());
        }

        [HttpDelete("{id, providerName}")]
        public void Delete(String userId, String providerName)
        {
            //_providerRepository.RemoveProvider(userId, providerName);
        }

        [HttpDelete("{userId}")]
        public void Delete(String userId)
        {
            //_providerRepository.RemoveProviders(userId);
        }
    }
}