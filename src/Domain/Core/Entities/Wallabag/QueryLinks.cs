using System.Text.Json.Serialization;

namespace Core.Entities.Wallabag
{
    public class QueryLinks : Links
    {
        [JsonPropertyName("first")] public HttpLink? First { get; set; } = null;
    
        [JsonPropertyName("last")] public HttpLink? Last { get; set; } = null;

        [JsonPropertyName("next")] public HttpLink? Next { get; set; } = null;
    }
}