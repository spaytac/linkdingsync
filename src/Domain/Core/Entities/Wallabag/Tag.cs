using System.Text.Json.Serialization;

namespace Core.Entities.Wallabag
{
    public class Tag
    {
        [JsonPropertyName("id")] public int Id { get; set; }

        [JsonPropertyName("label")] public string Label { get; set; }

        [JsonPropertyName("slug")] public string Slug { get; set; }
    }
}