using System.Text.Json.Serialization;

namespace TARge25Shop.Core.Dto.cocktail;

public class CocktailRootDto
{
    [JsonPropertyName("drinks")]
    public List<CocktailDto>? Drinks { get; set; }
}