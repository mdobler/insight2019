using System.Net.Http;
using System.Threading.Tasks;
using InsightAPISample.WebApp.Models;

namespace InsightAPISample.WebApp.Repositories
{
    public interface IRepository
    {
        Task<HttpClient> GetAuthorizedVantagePointClientAsync();
        HttpClient GetVantagePointClient();
        Task<TokenInfo> GetVantagePointTokenAsync(HttpClient client);
        Task<dynamic> GetAsync(HttpClient authorizedClient, string requestUri);
        Task<dynamic> PostAsync(HttpClient authorizedClient, string requestUri, dynamic data);
        Task<T> GetAsync<T>(HttpClient authorizedClient, string requestUri);
        Task<T> PostAsync<T>(HttpClient authorizedClient, string requestUri, dynamic data);

        Models.VantagePointSettings GetSettings();
        bool SaveSettings(Models.VantagePointSettings model);
    }
}