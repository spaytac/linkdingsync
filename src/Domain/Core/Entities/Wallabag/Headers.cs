using System.Text.Json.Serialization;

namespace Core.Entities.Wallabag
{
    public class Headers
    {
        [JsonPropertyName("server")] public string Server { get; set; }

        [JsonPropertyName("date")] public string Date { get; set; }

        [JsonPropertyName("content-type")] public string ContentType { get; set; }

        [JsonPropertyName("content-length")] public string ContentLength { get; set; }

        [JsonPropertyName("connection")] public string Connection { get; set; }

        [JsonPropertyName("x-powered-by")] public string XPoweredBy { get; set; }

        [JsonPropertyName("cache-control")] public string CacheControl { get; set; }

        [JsonPropertyName("etag")] public string Etag { get; set; }

        [JsonPropertyName("vary")] public string Vary { get; set; }

        [JsonPropertyName("strict-transport-security")]
        public string StrictTransportSecurity { get; set; }
    }
}