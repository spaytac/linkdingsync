using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Core.Entities.Wallabag;

namespace Core.Abstraction
{
    public interface IWallabagService
    {
        Task<AuthenticationHeaderValue> GetAuthenticationHeaderAsync(IEnumerable<string> scopes = null);

        Task<HttpResponseMessage> GetAsync(string endpoint, IEnumerable<string> scopes = null,
            bool httpCompletionResponseContentRead = false);

        Task<T> GetJsonAsync<T>(string endpoint, IEnumerable<string> scopes = null);

        Task<HttpResponseMessage> PostWithFormDataAsnyc(string endpoint, FormUrlEncodedContent content,
            IEnumerable<string> scopes = null);

        Task<HttpResponseMessage> PostAsync(string endpoint, HttpContent content,
            IEnumerable<string> scopes = null);

        Task<HttpResponseMessage> PostAsync(string endpoint, HttpRequestMessage request,
            IEnumerable<string> scopes = null);

        Task<HttpResponseMessage> PutAsync(string endpoint, HttpContent content,
            IEnumerable<string> scopes = null);

        Task<HttpResponseMessage> DeleteAsync(string endpoint, IEnumerable<string> scopes = null,
            HttpContent content = null);

        Task<IEnumerable<WallabagItem>> GetEntries(string format = "json", int limit = 50, bool full = false);
        Task<WallabagItem> GetEntryById(int id, string format = "json");
        Task<WallabagItem> AddEntryByUrl(string url, IEnumerable<string> tags = null, string format = "json");
    }
}

