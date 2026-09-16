using Microsoft.EntityFrameworkCore;
using TARge25Shop.Core;
using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;
using TARge25Shop.Data;

namespace TARge25Shop.ApplicationServices.Services;

public class KindergartenServices : IKindergartenServiceInterface
{
    private readonly TARge25ShopContext _dbContext;

    public KindergartenServices(TARge25ShopContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Kindergarten> Create(KindergartenDto dto)
    {
        var kindergarten = new Kindergarten
        {
            Id = Guid.NewGuid(),
            GroupName = dto.GroupName,
            KindergartenName = dto.KindergartenName,
            ChildrenCount = dto.ChildrenCount,
            TeacherName = dto.TeacherName,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
        };
        try
        {
            _dbContext.Add(kindergarten);
            await _dbContext.SaveChangesAsync();
            
            return kindergarten;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error saving new spaceship: {e.Message}");
            return null;
        }
    }

    public async Task<Kindergarten?> Update(KindergartenDto dto)
    {
        var kindergarten = new Kindergarten();

        kindergarten.Id = Guid.NewGuid();
        kindergarten.KindergartenName = dto.KindergartenName;
        kindergarten.ChildrenCount = dto.ChildrenCount;
        kindergarten.TeacherName = dto.TeacherName;
        kindergarten.CreatedAt = dto.CreatedAt;
        kindergarten.UpdatedAt = DateTime.Now;

        _dbContext.Kindergartens.Update(kindergarten);
        await _dbContext.SaveChangesAsync();

        return kindergarten;
    }

    public async Task<Kindergarten?> DetailAsync(Guid id)
    {
        var kindergarten = await _dbContext.Kindergartens
            .FirstOrDefaultAsync(x => x.Id == id);

        return kindergarten;
    }

    public async Task<Kindergarten?> Delete(Guid id)
    {
        var result = await DetailAsync(id);
        
        _dbContext.Kindergartens.Remove(result);
        await _dbContext.SaveChangesAsync();

        return result;
    }
}