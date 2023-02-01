using System.Text.Json.Serialization;

namespace Core.Entities.Linkding
{
    public class BookmarkUpdatePayload : BookmarkBase
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}