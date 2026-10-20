using System.Text.Json;
using Microsoft.Extensions.Logging;
using TARge25Shop.Core.Dto.cocktail;
using TARge25Shop.Core.ServiceInterface;

namespace TARge25Shop.ApplicationServices.Services;

public class CocktailServices : ICocktailServices
{
    // private readonly HttpClient _httpClient;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<CocktailServices>  _logger;

    public CocktailServices(IHttpClientFactory httpClientFactory, ILogger<CocktailServices> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<CocktailRootDto> CocktailResultHttpClient(string name)
    {
        _logger.LogInformation("API request started...");
        var requestUri = $"search.php?s={Uri.EscapeDataString(name)}";
        var client = _httpClientFactory.CreateClient("CocktailAPI");

        try
        {
            var response = await client.GetAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Request was successful: {JsonResponse}", jsonString);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var result = JsonSerializer.Deserialize<CocktailRootDto>(jsonString, options);
                return result;
            }
            else
            {
                _logger.LogWarning("API responded with failed status: {StatusCode}", response.StatusCode);
                return null;
            }
        }
        catch (HttpRequestException e)
        {
            _logger.LogError(e, "An exception occured while making the request to API.");
            return null;
        }
    }
}