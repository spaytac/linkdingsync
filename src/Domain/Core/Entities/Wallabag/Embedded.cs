using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Core.Entities.Wallabag
{
    public class Embedded
    {
        [JsonPropertyName("items")]
        public List<WallabagItem> Items { get; set; }
    }
}