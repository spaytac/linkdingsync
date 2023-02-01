using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wallabag.Client.Converters
{
    public class DateTimeConverterForCustomStandard : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateTimeString = reader.GetString();

            if (string.IsNullOrEmpty(dateTimeString))
            {
                return DateTime.MinValue;
            }
            DateTime dt = DateTime.ParseExact(dateTimeString, "yyyy-MM-dd'T'HH:mm:ssK",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal);

            return dt;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}