using Microsoft.EntityFrameworkCore;
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
    
    public async Task<Spaceship> Create(SpaceshipDto spaceshipDto)
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

    public async Task<Spaceship?> Update(SpaceshipDto dto)
    {
        var spaceship = new Spaceship();

        spaceship.Id = dto.Id;
        spaceship.Name = dto.Name;
        spaceship.ShipType = dto.ShipType;
        spaceship.MaxCrewSize = dto.MaxCrewSize;
        spaceship.EnginePower = dto.EnginePower;
        spaceship.CreatedAt = dto.CreatedAt;
        spaceship.UpdatedAt = DateTime.Now;
        
        _dbContext.Spaceships.Update(spaceship);
        await _dbContext.SaveChangesAsync();
           
        return spaceship;
    }
    
    public async Task<Spaceship?> DetailAsync(Guid id)
    {
        var spaceship = await _dbContext.Spaceships
            .FirstOrDefaultAsync(x => x.Id == id);

        return spaceship;
    }

    public async Task<Spaceship?> Delete(Guid id)
    {
        var result = await DetailAsync(id);

        _dbContext.Spaceships.Remove(result);
        await _dbContext.SaveChangesAsync();

        return result;
    }
    
}