using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackendProjem.Domain;
using BackendProjem.Domain.Entities;

namespace BackendProjem.Infrastructure
{
    public interface ITestUserService
    {
        /// <summary>
        /// TestUser tablosuna kayıt atılmasını sağlar
        /// </summary>
        /// <returns>İşlemin başarılı olup olmadığı bilgisini döner</returns>
        Task<bool> AddTestUser(TestUser testUser);
        Task<bool> UpdateUser(TestUserUpdateRequestModel testUserUpdateRequestModel);
        Task<bool> RemoveUser(int id);
        Task<List<TestUser>> GetUser();
        Task<bool> CheckUserByEmail(string email, string password);
        Task<TestUser> GetUserById(int id);
    }
}
