using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;

namespace TARge25Shop.Core;

public interface IFileServices
{
    public void ConvertFilesToApi(SpaceshipDto dto, Spaceship spaceship);
    
}