using Wallabag.Client.Contracts;

namespace Wallabag.Client.OAuth;

public class OAuthTokenProvider : IAccessTokenProvider
{
    protected record TokenCache(string access_token, DateTime expires);

    private readonly AuthenticationClient _client;
    
    protected Dictionary<string, TokenCache> cache = new ();

    public OAuthTokenProvider(AuthenticationClient client)
    {
        _client = client;
    }

    public async Task<string> GetToken(IEnumerable<string> scopes = null)
    {
        string cacheKey = "wallabag+token";

        if (scopes != null && scopes.Count() > 0)
        {
            cacheKey = string.Join('+', scopes);
        }
        
        if (cache.ContainsKey(cacheKey))
        {
            var tokenCache = cache[cacheKey];
            if (tokenCache.expires > DateTime.Now)
            {
                return tokenCache.access_token;
            }
            else
            {
                cache.Remove(cacheKey);
            }
        }
        var auth = await _client.Authenticate(scopes);
        cache.Add(cacheKey, new TokenCache(auth.access_token, DateTime.Now.AddSeconds(auth.expires_in)));
        return auth.access_token;
    }
}