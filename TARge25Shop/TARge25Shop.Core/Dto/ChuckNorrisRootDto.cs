using System.Text.Json.Serialization;
using TARge25Shop.Core.Helpers;

namespace TARge25Shop.Core.Dto;

public class ChuckNorrisRootDto
{
    [JsonPropertyName("categories")]
    public List<string> Categories { get; set; } = new();

    [JsonPropertyName("created_at")]
    [JsonConverter(typeof(CustomDateTimeOffsetConverter))]
    public DateTimeOffset CreatedAt { get; set; }
    
    [JsonPropertyName("icon_url")]
    public string IconUrl { get; set; } = string.Empty;
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    
    [JsonPropertyName("updated_at")]
    [JsonConverter(typeof(CustomDateTimeOffsetConverter))]
    public DateTimeOffset UpdatedAt { get; set; }
    
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;
}