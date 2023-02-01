using System.Text.Json.Serialization;

namespace Core.Entities.Wallabag
{
    public class Range
    {
        [JsonPropertyName("start")] public string Start { get; set; }

        [JsonPropertyName("startOffset")] public string StartOffset { get; set; }

        [JsonPropertyName("end")] public string End { get; set; }

        [JsonPropertyName("endOffset")] public string EndOffset { get; set; }
    }
}