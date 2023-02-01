using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Core.Abstraction;
using Microsoft.Extensions.Options;
using Wallabag.Client.Contracts;
using Wallabag.Client.Converters;
using Wallabag.Client.Options;

namespace Wallabag.Client;

public partial class WallabagService : IWallabagService
{
    private readonly WallabagSettings _settings;
    private IAccessTokenProvider _accessTokenProvider;
    public readonly HttpClient _client;

    public WallabagService(HttpClient client, IOptions<WallabagSettings> settings,
        IAccessTokenProvider accessTokenProvider)
    {
        _client = client;
        _accessTokenProvider = accessTokenProvider;
        _settings = settings.Value;
        _client.BaseAddress = new Uri(_settings.Url);
    }

    public async Task<AuthenticationHeaderValue> GetAuthenticationHeaderAsync(IEnumerable<string> scopes = null)
    {
        return new AuthenticationHeaderValue("Bearer", await _accessTokenProvider.GetToken(scopes));
    }

    public async Task<HttpResponseMessage> GetAsync(string endpoint, IEnumerable<string> scopes = null,
        bool httpCompletionResponseContentRead = false)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        request.Headers.Authorization = await GetAuthenticationHeaderAsync(scopes);

        HttpResponseMessage response = null;

        if (httpCompletionResponseContentRead)
        {
            response = await _client.SendAsync(request, HttpCompletionOption.ResponseContentRead);
        }
        else
        {
            response = await _client.SendAsync(request);
        }

        response.EnsureSuccessStatusCode();
        return response;
    }

    public async Task<T> GetJsonAsync<T>(string endpoint, IEnumerable<string> scopes = null)
    {
        var response = await GetAsync(endpoint, scopes);

        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<HttpResponseMessage> PostWithFormDataAsnyc(string endpoint, FormUrlEncodedContent content,
        IEnumerable<string> scopes = null)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
        request.Content = content;
        
        return await PostAsync(endpoint, request, scopes);
    }

    public async Task<HttpResponseMessage> PostAsync(string endpoint, HttpContent content,
        IEnumerable<string> scopes = null)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
        request.Content = content;

        return await PostAsync(endpoint, request, scopes);
    }

    public async Task<HttpResponseMessage> PostAsync(string endpoint, HttpRequestMessage request,
        IEnumerable<string> scopes = null)
    {
        request.Headers.Authorization = await GetAuthenticationHeaderAsync(scopes);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return response;
    }

    public async Task<HttpResponseMessage> PutAsync(string endpoint, HttpContent content,
        IEnumerable<string> scopes = null)
    {
        using var request = new HttpRequestMessage(HttpMethod.Put, endpoint);
        request.Headers.Authorization = await GetAuthenticationHeaderAsync(scopes);
        request.Content = content;
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return response;
    }

    public async Task<HttpResponseMessage> DeleteAsync(string endpoint, IEnumerable<string> scopes = null,
        HttpContent content = null)
    {
        using var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);
        request.Headers.Authorization = await GetAuthenticationHeaderAsync(scopes);
        request.Content = content;
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return response;
    }
}