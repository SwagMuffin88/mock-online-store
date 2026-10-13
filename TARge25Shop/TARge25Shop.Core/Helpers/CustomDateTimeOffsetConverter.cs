namespace TARge25Shop.Core.Helpers;

using System.Buffers;
using System.Text.Json;
using System.Text.Json.Serialization;

public class CustomDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? dateString = reader.GetString();

        if (string.IsNullOrWhiteSpace(dateString))
        {
            return DateTimeOffset.MinValue;
        }

        string formattedString = dateString.Replace(" ", "T");

        if (DateTimeOffset.TryParse(formattedString, out var dateTimeOffset))
        {
            return dateTimeOffset;
        }

        throw new JsonException($"Could not parse date: '{dateString}'");
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFZ"));
    }
}