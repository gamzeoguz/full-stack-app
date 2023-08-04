using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendProjem.Infrastructure
{
    public interface ITokenService
    {
        Task<string> GetToken(string email, string password);
       

    }
}
