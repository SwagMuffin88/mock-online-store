using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using TARge25Shop.Core.Dto.Weather;
using TARge25Shop.Core.ServiceInterface;

namespace TARge25Shop.ApplicationServices.Services;

public class WeatherService : IWeatherService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<WeatherService> _logger;
    private readonly string _apiKey;
    
    public WeatherService(IHttpClientFactory httpClientFactory, ILogger<WeatherService> logger)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        
        _apiKey = Environment.GetEnvironmentVariable("OPENWEATHER_API_KEY");
    }

    public async Task<WeatherResponseDto?> GetWeatherAsync(string cityName)
    {
        _logger.LogInformation("API request started...");
        
        if (string.IsNullOrEmpty(_apiKey))
        {
            throw new InvalidOperationException("No API key found in .env file.");
        }

        var client = _httpClientFactory.CreateClient("OpenWeatherMapAPI");

        try
        {
            string request = $"weather?q={cityName}&appid={_apiKey}&units=metric&lang=et";

            var response = await client.GetAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("API responded with failed status: {StatusCode}", response.StatusCode);
                return null;
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<WeatherResponseDto>(jsonString, options);
        }
        catch (HttpRequestException e)
        {
            _logger.LogError(e, "An exception occured while making the request to API.");
            return null;
        }
    }
}