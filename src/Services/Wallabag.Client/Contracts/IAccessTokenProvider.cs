namespace Wallabag.Client.Contracts;

public interface IAccessTokenProvider
{
    Task<string> GetToken(IEnumerable<string> scopes);
}