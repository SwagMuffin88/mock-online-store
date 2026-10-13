using TARge25Shop.Core.Dto;

namespace TARge25Shop.Core;

public interface IChuckNorrisServices
{
    Task<ChuckNorrisRootDto?> ChuckNorrisResultHttpClient();
}