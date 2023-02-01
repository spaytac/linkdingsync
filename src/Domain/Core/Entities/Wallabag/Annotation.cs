using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Wallabag.Client.Converters;

namespace Core.Entities.Wallabag
{
    public class Annotation
    {
        [JsonPropertyName("user")] public string User { get; set; }

        [JsonPropertyName("annotator_schema_version")]
        public string AnnotatorSchemaVersion { get; set; }

        [JsonPropertyName("id")] public int Id { get; set; }

        [JsonPropertyName("text")] public string Text { get; set; }


        [JsonConverter(typeof(DateTimeConverterForCustomStandard))]
        [JsonPropertyName("created_at")] public DateTime? CreatedAt { get; set; } = null;

        [JsonConverter(typeof(DateTimeConverterForCustomStandard))]
        [JsonPropertyName("updated_at")] public DateTime? UpdatedAt { get; set; } = null;

        [JsonPropertyName("quote")] public string Quote { get; set; }

        [JsonPropertyName("ranges")] public List<Range> Ranges { get; set; }
    }
}