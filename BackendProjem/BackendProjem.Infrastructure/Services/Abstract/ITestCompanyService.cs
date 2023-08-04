using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackendProjem.Domain;
using BackendProjem.Domain.Entities;
using BackendProjem.Domain.Model;

namespace BackendProjem.Infrastructure
{
    public interface ITestCompanyService
    {
        Task<List<CompanyMappingType>> GetCompany();
        Task<List<TestCompany>> GetCompanyFromElasticSearch(String companyName);
        Task<bool> RemoveTestCompany(int id);
        Task<bool> UpdateTestCompany(TestCompanyUpdateRequestModel testCompanyUpdateRequestModel);

    }
}
