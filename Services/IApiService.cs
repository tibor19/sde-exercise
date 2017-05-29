using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreOAuth2Sample.Model;

namespace AspNetCoreOAuth2Sample.Services
{
    public interface IApiService
    {
        Task<IEnumerable<Rule>> GetAllRules();
        Task<IEnumerable<string>> GetAllClients();
    }
}