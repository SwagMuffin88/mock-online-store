using TARge25Shop.Core.Dto.cocktail;

namespace TARge25Shop.Core.ServiceInterface;

public interface ICocktailServices
{
    Task<CocktailRootDto> GetCocktailsFromHttpClient(string name);}