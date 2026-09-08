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
            Id = Guid.NewGuid(),
            /* Märkus:
             * Kui Spaceship klassis on Id tüübiks Guid, genereerib Entity Framework Core sellele ise uue
             * väärtuse (Guid.NewGuid()), kui sa seda ise koodis ei määra. Seega käsitsi spaceship.Id = Guid.NewGuid();
             * kirjutamine ei ole tegelikult kohustuslik, kuid koodis pole see ka viga.
             */
            
            Name = spaceshipDto.Name,
            ShipType = spaceshipDto.ShipType,
            MaxCrewSize = spaceshipDto.MaxCrewSize,
            EnginePower = spaceshipDto.EnginePower,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        try
        {
            _dbContext.Add(spaceship);
            await _dbContext.SaveChangesAsync();
            
            return spaceship;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error saving new spaceship: {e.Message}");
            return null;
        }
    }
}