using Microsoft.AspNetCore.Mvc;
using TARge25Shop.ApplicationServices.Services;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Models.Weather;

namespace TARge25Shop.Controllers;

public class WeatherController : Controller
{
    private readonly IWeatherService _weatherService;

    private static readonly List<string> Cities = new()
    {
        "Tallinn", "Tartu", "Helsinki", "Stockholm", "London", "New York", "Tokyo"
    };

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string selectedCity = "Tallinn")
    {
        var viewModel = new WeatherViewModel
        {
            AvailableCities = Cities,
            SelectedCity = selectedCity
        };

        var weatherDto = await _weatherService.GetWeatherAsync(selectedCity);

        if (weatherDto == null)
        {
            ModelState.AddModelError("", "Could not fetch weather data or city was not found.");
            return View(viewModel);
        }

        viewModel.CityName = weatherDto.Name;
        viewModel.Temperature = weatherDto.Main.Temp;
        viewModel.Humidity = weatherDto.Main.Humidity;
        viewModel.Description = weatherDto.Weather.FirstOrDefault()?.Description ?? "";

        return View(viewModel);
    }
}