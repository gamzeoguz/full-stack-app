using BackendProjem.Domain;
using BackendProjem.Domain.Entities;
using BackendProjem.Domain.Model;
using BackendProjem.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BackendProjem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestCompanyController : ControllerBase
    {
        private readonly ITestCompanyService _testCompanyService;
        public TestCompanyController(ITestCompanyService testCompanyService)
        {
            this._testCompanyService = testCompanyService;
        }

        [HttpGet("Get/{companyName}")]
        public async Task<List<TestCompany>> GetCompanies(String companyName)
        {
            var result = await this._testCompanyService.GetCompanyFromElasticSearch(companyName).ConfigureAwait(false);
            return result;
        }

        [HttpDelete("Remove/{id}")]
        public async Task<bool> Remove(int id)
        {
            return await this._testCompanyService.RemoveTestCompany(id).ConfigureAwait(false);
        }

        [HttpPut("Update")]
        public async Task<bool> Update([FromBody] TestCompanyUpdateRequestModel testCompanyUpdateRequestModel)
        {
            return await this._testCompanyService.UpdateTestCompany(testCompanyUpdateRequestModel).ConfigureAwait(false);
        }
    }
}

