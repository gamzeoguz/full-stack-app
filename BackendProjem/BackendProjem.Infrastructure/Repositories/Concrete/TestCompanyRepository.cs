using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackendProjem.Domain.Entities;
using BackendProjem.Infrastructure;
using BackendProjem.Infrastructure.Context;

namespace BackendProjem.Infrastructure
{
    public class TestCompanyRepository : SqlRepository<TestCompany, CompanyDbContext>, ITestCompanyRepository
    {
        public TestCompanyRepository(CompanyDbContext context) : base(context)
        {
        }
    }
}