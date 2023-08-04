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
    public class TestUserRepository : SqlRepository<TestUser, CompanyDbContext>, ITestUserRepository
    {
        public TestUserRepository(CompanyDbContext context) : base(context)
        {
        }
    }
}
