namespace Wallabag.Client.Options;

public class WallabagSettings
{
    public const string Position = "Wallabag";

    public string Url { get; set; } = "https://app.wallabag.it";
    public string Username { get; set; }
    public string Password { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string GrandType { get; set; } = "password";
}