namespace Linkding.Client.Options;

public class LinkdingSettings
{
    public const string Position = "Linkding";
    
    public string Key { get; set; }
    public string Url { get; set; }
    public bool UpdateBookmarks { get; set; } = true;
}