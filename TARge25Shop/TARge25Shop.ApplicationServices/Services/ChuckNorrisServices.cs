using System.Net.Http.Json;
using System.Text.Json;
using TARge25Shop.Core;
using TARge25Shop.Core.Dto;
using Microsoft.Extensions.Logging;

namespace TARge25Shop.ApplicationServices.Services;

public class ChuckNorrisServices : IChuckNorrisServices
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ChuckNorrisServices> _logger;

    public ChuckNorrisServices(IHttpClientFactory httpClientFactory, ILogger<ChuckNorrisServices> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }
    
    public async Task<ChuckNorrisRootDto?> ChuckNorrisResultHttpClient()
    {
        _logger.LogInformation("API request started...");
        var client = _httpClientFactory.CreateClient("ChuckNorrisJokesApi");
        
        try
        {
            
            var response = await client.GetAsync("jokes/random");

            if (response.IsSuccessStatusCode)
            {
                //var result = await response.Content.ReadFromJsonAsync<ChuckNorrisRootDto>();
                var jsonString = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Request was successful: {JsonResponse}", jsonString);

                var result = JsonSerializer.Deserialize<ChuckNorrisRootDto>(jsonString);
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