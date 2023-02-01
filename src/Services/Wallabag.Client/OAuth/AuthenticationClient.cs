using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Wallabag.Client.Options;

namespace Wallabag.Client.OAuth;

public record WallabagToken(string token_type, string access_token, string refresh_token, int expires_in, string scope);

public class AuthenticationClient
{
    protected const string AuthPath = "/oauth/v2/token";
    
    public readonly HttpClient _client;
    public readonly WallabagSettings _settings;

    public AuthenticationClient(HttpClient client, IOptions<WallabagSettings> settings)
    {
        _client = client;
        _settings = settings.Value;
        // var baseAdress = new Uri(_settings.Url);
        // client.BaseAddress = baseAdress.Append(AuthPath);
        client.BaseAddress = new Uri(_settings.Url);
    }
    
    public async Task<WallabagToken> Authenticate(IEnumerable<string> scopes = null)
    {
        var parameters = new List<KeyValuePair<string, string>>
        {
            new("client_id", _settings.ClientId),
            new("client_secret", _settings.ClientSecret),
            new("grant_type", _settings.GrandType),
            new("username", _settings.Username),
            new("password", _settings.Password)
        };

        if (scopes != null && scopes.Count() > 0)
        {
            parameters.Add(new("scope", string.Join(" ", scopes)));
        }
        var response = await _client.PostAsync("oauth/v2/token", new FormUrlEncodedContent(parameters));
        
        response.EnsureSuccessStatusCode();
        var authentication = await response.Content.ReadFromJsonAsync<WallabagToken>();
        if (authentication == null)
        {
            throw new HttpRequestException("Could not retrieve authentication data.");
        }
        return authentication;
    }
    public async Task<WallabagToken> RefreshToken(string refreshToken, IEnumerable<string> scopes = null)
    {
        var parameters = new List<KeyValuePair<string, string>>
        {
            new("client_id", _settings.ClientId),
            new("client_secret", _settings.ClientSecret),
            new("grant_type", "refresh_token"),
            new("refresh_token", refreshToken),
            new("username", _settings.Username),
            new("password", _settings.Password)
        };

        if (scopes != null && scopes.Count() > 0)
        {
            parameters.Add(new("scope", string.Join(" ", scopes)));
        }
        
        var response = await _client.PostAsync("oauth/v2/token", new FormUrlEncodedContent(parameters));
        
        response.EnsureSuccessStatusCode();
        var authentication = await response.Content.ReadFromJsonAsync<WallabagToken>();
        if (authentication == null)
        {
            throw new HttpRequestException("Could not retrieve authentication data.");
        }
        return authentication;
    }
}