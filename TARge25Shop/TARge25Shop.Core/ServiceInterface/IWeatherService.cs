using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto.Weather;

namespace TARge25Shop.Core.ServiceInterface;

public interface IWeatherService
{
    Task<WeatherResponseDto?> GetWeatherAsync(string cityName);
}