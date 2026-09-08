using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;

namespace TARge25Shop.Core;

public interface ISpaceshipServiceInterface
{
    Task<Spaceship> Create(SpaceshipDto dto);
}