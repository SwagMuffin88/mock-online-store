using System.Text.Json.Serialization;

namespace TARge25Shop.Core.Dto.Weather;

public class WeatherResponseDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("main")]
    public MainDto Main { get; set; }

    [JsonPropertyName("weather")]
    public WeatherDescriptionDto[] Weather { get; set; }
}

public class MainDto
{
    [JsonPropertyName("temp")]
    public double Temp { get; set; }

    [JsonPropertyName("humidity")]
    public int Humidity { get; set; }
}

public class WeatherDescriptionDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; }
}