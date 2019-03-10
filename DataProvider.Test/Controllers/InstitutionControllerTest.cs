using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataProvider.Controllers;
using GoldMountainShared.Dto;
using GoldMountainShared.Dto.Provider;
using GoldMountainShared.Storage.Repositories;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace DataProvider.Test.Controllers
{
    public class InstitutionControllerTest : ControllerTestBase, IDisposable
    {
        private readonly InstitutionController _controller;

        public InstitutionControllerTest()
        {
            var institutionRepository = new InstitutionRepository(_options);
            _controller = new InstitutionController(institutionRepository);
        }

        [Fact]
        public void GetTest_Success()
        {
            InitializeMapper();

            var postResult = _controller.Post();
            var getResult = _controller.Get().Result;
            var okObjectResult = getResult as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var institutions = okObjectResult.Value as IList<InstitutionDto>;
            Assert.NotNull(institutions);
            Assert.NotEmpty(institutions);

            foreach (var institution in institutions)
            {
                Assert.NotEmpty(institution.Name);
                Assert.NotEmpty(institution.Credentials);
            }
        }

        [Fact]
        public void PostTest_Success()
        {
            InitializeMapper();

            var result = _controller.Post().Result;
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var institutions = okObjectResult.Value as IList<InstitutionDto>;

            Assert.NotNull(institutions);
            Assert.NotEmpty(institutions);
        }

        private void CleanInstitutionRepository()
        {
            var institutionRepository = new InstitutionRepository(_options);
            institutionRepository.RemoveAllInstitutions();
        }

        public void Dispose()
        {
            CleanInstitutionRepository();
        }
    }
}
