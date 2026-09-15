using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;

namespace TARge25Shop.Core;

public interface ISpaceshipServiceInterface
{
    Task<Spaceship> Create(SpaceshipDto dto);
    Task<Spaceship?> Update(SpaceshipDto dto);
    Task<Spaceship> DetailAsync(Guid id);
    Task<Spaceship?> Delete(Guid id);
}