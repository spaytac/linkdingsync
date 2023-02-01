using System.Text.Json.Serialization;

namespace Core.Entities.Linkding
{
    public class TagCreatePayload
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}