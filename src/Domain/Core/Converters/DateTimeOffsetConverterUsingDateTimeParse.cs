using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wallabag.Client.Converters
{
    public class DateTimeOffsetConverterUsingDateTimeParse : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateTimeString = reader.GetString();
            return DateTimeOffset.Parse(dateTimeString);
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}