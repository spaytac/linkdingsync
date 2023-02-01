using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Wallabag.Client.Converters;

namespace Core.Entities.Wallabag
{
    public class WallabagItem
{
    [JsonPropertyName("is_archived")] public int IsArchived { get; set; }

    [JsonPropertyName("is_starred")] public int IsStarred { get; set; }

    [JsonPropertyName("user_name")] public string UserName { get; set; }

    [JsonPropertyName("user_email")] public string UserEmail { get; set; }

    [JsonPropertyName("user_id")] public int UserId { get; set; }

    [JsonPropertyName("tags")] public List<Tag> Tags { get; set; }

    [JsonPropertyName("is_public")] public bool IsPublic { get; set; }

    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("uid")] public string? Uid { get; set; } = null;

    [JsonPropertyName("title")] public string Title { get; set; }

    [JsonPropertyName("url")] public string Url { get; set; }

    [JsonPropertyName("hashed_url")] public string HashedUrl { get; set; }

    [JsonPropertyName("origin_url")] public string? OriginUrl { get; set; } = null;

    [JsonPropertyName("given_url")] public string GivenUrl { get; set; }

    [JsonPropertyName("hashed_given_url")] public string HashedGivenUrl { get; set; }

    [JsonConverter(typeof(DateTimeConverterForCustomStandard))]
    [JsonPropertyName("archived_at")] public DateTime? ArchivedAt { get; set; } = null;

    [JsonPropertyName("content")] public string? Content { get; set; } = null;

    [JsonConverter(typeof(DateTimeConverterForCustomStandard))]
    [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }

    [JsonConverter(typeof(DateTimeConverterForCustomStandard))]
    [JsonPropertyName("updated_at")] public DateTime UpdatedAt { get; set; }

    [JsonConverter(typeof(DateTimeConverterForCustomStandard))]
    [JsonPropertyName("published_at")] public DateTime PublishedAt { get; set; }

    [JsonPropertyName("published_by")] public List<string> PublishedBy { get; set; }

    [JsonConverter(typeof(DateTimeConverterForCustomStandard))]
    [JsonPropertyName("starred_at")] public DateTime? StarredAt { get; set; } = null;

    [JsonPropertyName("annotations")] public List<Annotation> Annotations { get; set; }

    [JsonPropertyName("mimetype")] public string Mimetype { get; set; }

    [JsonPropertyName("language")] public string Language { get; set; }

    [JsonPropertyName("reading_time")] public int ReadingTime { get; set; }

    [JsonPropertyName("domain_name")] public string DomainName { get; set; }

    [JsonPropertyName("preview_picture")] public string PreviewPicture { get; set; }

    [JsonPropertyName("http_status")] public string HttpStatus { get; set; }

    [JsonPropertyName("headers")] public Headers Headers { get; set; }

    [JsonPropertyName("_links")] public Links Links { get; set; }
}
}