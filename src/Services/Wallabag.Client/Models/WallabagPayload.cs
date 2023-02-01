namespace Wallabag.Client.Models;

public class WallabagPayload
{
    public string Url { get; set; }
    public string Title { get; set; }
    public string Tags { get; set; }
    public int Archive { get; set; }
    public int Starred { get; set; }
    public string Content { get; set; }
    public string Language { get; set; }
    public string PreviewPicture { get; set; }
    public DateTime PublishedAt { get; set; }
    public string Authors { get; set; }
    public int Publich { get; set; }
    public string OriginUrl { get; set; } 
}