namespace TARge25Shop.Models.Cocktail;

public class CocktailSearchViewModel
{
    public string? SearchName { get; set; }

    public List<CocktailDetailsViewModel> Cocktails { get; set; } = new List<CocktailDetailsViewModel>();
}