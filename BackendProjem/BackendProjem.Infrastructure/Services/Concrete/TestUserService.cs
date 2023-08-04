using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackendProjem.Caching.Core;
using BackendProjem.Domain;
using BackendProjem.Domain.Entities;
using BackendProjem.Infrastructure;

namespace BackendProjem.Infrastructure;

public class TestUserService: ITestUserService
{
    private readonly ITestUserRepository _repository;
    private readonly ICacheManager _cacheManager;
    public TestUserService(ITestUserRepository repository, ICacheManager cacheManager)
    {
        this._repository = repository;
        this._cacheManager = cacheManager;

    }

    public async Task<bool> AddTestUser(TestUser testUser)
    {
        try
        {
            await this._repository.Insert(testUser).ConfigureAwait(false);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> UpdateUser(TestUserUpdateRequestModel testUserUpdateRequestModel)
    {
        try
        {
            var item = await this._repository.FirstOrDefault(x => x.Id == testUserUpdateRequestModel.Id).ConfigureAwait(false);
            if (item == null) { return false; }
            item.Name = testUserUpdateRequestModel.Name;
            await this._repository.Update(item).ConfigureAwait(false);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> RemoveUser(int id)
    {
        try
        {
            await this._repository.Remove(x => x.Id == id).ConfigureAwait(false);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<TestUser> GetUserById(int id)
    {
        try
        {
            var cacheResponse = await this._cacheManager.GetAsync<TestUser>($"TestUser-{id}", CacheRegion.Default, CachePlatform.InMemory).ConfigureAwait(false);
            if (cacheResponse != null)
            {
                return cacheResponse;
            }
            var testUser =  await this._repository.FirstOrDefault(x => x.Id == id).ConfigureAwait(false);
            await this._cacheManager.SetAsync($"TestUser-{id}", testUser, CacheRegion.Default, CacheDuration.FortyFiveMinutes, CachePlatform.InMemory).ConfigureAwait(false);
            return testUser;
        }
        catch (Exception e)
        {
            return null;
        }
    }


    public async Task<List<TestUser>> GetUser()
    {
        try
        {
            //x => x.Name == name
            var response = await this._repository.FindList().ConfigureAwait(false);
            return response;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public async Task<bool> CheckUserByEmail(string email, string password)
    {
        try
        {
            //x => x.Name == name
            var response = await this._repository.FirstOrDefault(x => x.Email == email && x.Password == password).ConfigureAwait(false);
            return response!=null;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}
