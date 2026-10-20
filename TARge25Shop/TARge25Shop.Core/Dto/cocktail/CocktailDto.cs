using System.Text.Json.Serialization;

namespace TARge25Shop.Core.Dto.cocktail;

public class CocktailDto
{
    [JsonPropertyName("idDrink")]
    public string? IdDrink { get; set; }

    [JsonPropertyName("strDrink")]
    public string? StrDrink { get; set; }

    [JsonPropertyName("strCategory")]
    public string? StrCategory { get; set; }

    [JsonPropertyName("strAlcoholic")]
    public string? StrAlcoholic { get; set; }

    [JsonPropertyName("strGlass")]
    public string? StrGlass { get; set; }

    [JsonPropertyName("strInstructions")]
    public string? StrInstructions { get; set; }

    [JsonPropertyName("strDrinkThumb")]
    public string? StrDrinkThumb { get; set; }

    [JsonPropertyName("strIngredient1")]
    public string? StrIngredient1 { get; set; }

    [JsonPropertyName("strIngredient2")]
    public string? StrIngredient2 { get; set; }

    [JsonPropertyName("strIngredient3")]
    public string? StrIngredient3 { get; set; }

    [JsonPropertyName("strIngredient4")]
    public string? StrIngredient4 { get; set; }

    [JsonPropertyName("strMeasure1")]
    public string? StrMeasure1 { get; set; }

    [JsonPropertyName("strMeasure2")]
    public string? StrMeasure2 { get; set; }

    [JsonPropertyName("strMeasure3")]
    public string? StrMeasure3 { get; set; }

    [JsonPropertyName("strMeasure4")]
    public string? StrMeasure4 { get; set; }
}