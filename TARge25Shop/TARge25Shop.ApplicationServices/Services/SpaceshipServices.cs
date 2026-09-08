using TARge25Shop.Core;
using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;
using TARge25Shop.Data;

namespace TARge25Shop.ApplicationServices.Services;


public class SpaceshipServices : ISpaceshipServiceInterface
{
    private readonly TARge25ShopContext _dbContext;

    public SpaceshipServices(TARge25ShopContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Spaceship?> Create(SpaceshipDto spaceshipDto)
    {
        var spaceship = new Spaceship
        {
            //Id = spaceshipDto.Id, -> works either way? kusagil genereerib automaatselt isegi ilma meetodita.
            Id = Guid.NewGuid(),
            Name = spaceshipDto.Name,
            ShipType = spaceshipDto.ShipType,
            MaxCrewSize = spaceshipDto.MaxCrewSize,
            EnginePower = spaceshipDto.EnginePower,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        _dbContext.Add(spaceship);
        await _dbContext.SaveChangesAsync();

        return spaceship;
    }
}