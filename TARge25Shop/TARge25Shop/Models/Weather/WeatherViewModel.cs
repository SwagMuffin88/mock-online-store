namespace TARge25Shop.Models.Weather;

public class WeatherViewModel
{
    public string CityName { get; set; } = string.Empty;
    public double Temperature { get; set; }
    public int Humidity { get; set; }
    public string Description { get; set; } = string.Empty;

    public List<string> AvailableCities { get; set; } = new();
    public string SelectedCity { get; set; } = string.Empty;
}