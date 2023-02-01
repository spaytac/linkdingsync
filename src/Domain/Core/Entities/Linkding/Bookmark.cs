using System;
using System.Text.Json.Serialization;

namespace Core.Entities.Linkding
{
    public class Bookmark : BookmarkBase
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
  
        [JsonPropertyName("website_title")]
        public string WebsiteTitle { get; set; }
        [JsonPropertyName("website_description")]
        public string WebsiteDescription { get; set; }
        [JsonPropertyName("is_archived")]
        public bool IsArchived { get; set; }
        [JsonPropertyName("unread")]
        public bool Unread { get; set; }
        [JsonPropertyName("date_added")]
        public DateTime DateAdded { get; set; }
        [JsonPropertyName("date_modified")]
        public DateTime DateModified { get; set; }
    }
}

// "id": 1,
// "url": "https://example.com",
// "title": "Example title",
// "description": "Example description",
// "website_title": "Website title",
// "website_description": "Website description",
// "is_archived": false,
// "unread": false,
// "tag_names": [
// "tag1",
// "tag2"
//     ],
// "date_added": "2020-09-26T09:46:23.006313Z",
// "date_modified": "2020-09-26T16:01:14.275335Z"