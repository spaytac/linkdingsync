using System.Text.Json.Serialization;

namespace Core.Entities.Wallabag
{
    public class HttpLink
    {
        [JsonPropertyName("href")] public string Href { get; set; }
    }
}