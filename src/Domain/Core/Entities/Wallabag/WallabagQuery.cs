using System.Text.Json.Serialization;

namespace Core.Entities.Wallabag
{
    public class WallabagQuery
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("pages")]
        public int Pages { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("_links")]
        public QueryLinks QueryLinks { get; set; }

        [JsonPropertyName("_embedded")]
        public Embedded Embedded { get; set; }
    }
}