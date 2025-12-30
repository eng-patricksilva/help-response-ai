using HelpResponse.AI.Infra.ClientHttp.ClaudiaDbClientHttp.Searches;
using Refit;
using System.Threading.Tasks;

namespace HelpResponse.AI.Infra.ClientHttp.ClaudiaDbClientHttp
{
    public interface IClaudiaDbClientApi
    {
        [Post("/claudia-ids-index-large/docs/search?api-version=2023-11-01")]
        Task<SearchOutput> Search([Body] SearchInput input);
    }
}