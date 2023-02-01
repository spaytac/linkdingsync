using System.Text.Json.Serialization;

namespace Core.Entities.Wallabag
{
    public class Links
    {
        [JsonPropertyName("self")] public HttpLink? Self { get; set; }  = null;
    }
}