using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using TARge25Shop.Core;
using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;
using TARge25Shop.Data;

namespace TARge25Shop.ApplicationServices.Services;

public class KindergartenServices : IKindergartenServiceInterface
{
    private readonly TARge25ShopContext _dbContext;
    private readonly IFileServices  _fileServices;

    public KindergartenServices(TARge25ShopContext dbContext, IFileServices fileServices)
    {
        _dbContext = dbContext;
        _fileServices = fileServices;
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

        _fileServices.UploadFilesToApi(dto, kindergarten);
        
        try
        {
            _dbContext.Add(kindergarten);
            await _dbContext.SaveChangesAsync();
            
            return kindergarten;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error saving new kindergarten: {e.Message}");
            return null;
        }
    }

    public async Task<Kindergarten?> Update(KindergartenDto dto)
    {
        var kindergarten = new Kindergarten();

        kindergarten.Id = dto.Id;
        kindergarten.GroupName = dto.GroupName;
        kindergarten.KindergartenName = dto.KindergartenName;
        kindergarten.ChildrenCount = dto.ChildrenCount;
        kindergarten.TeacherName = dto.TeacherName;
        kindergarten.CreatedAt = dto.CreatedAt;
        kindergarten.UpdatedAt = DateTime.Now;
        _fileServices.UploadFilesToApi(dto, kindergarten);

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

        if (result == null)
        {
            return null;
        }
        
        var images = await _dbContext.FilesToApis
            .Where(x => x.ObjectId == id)
            .Select(y => new FileToApiDto
            {
                Id = y.Id,
                ObjectId = y.ObjectId,
                ExistingFilePath = y.ExistingFilePath
            }).ToArrayAsync();
        
        try
        {
            await _fileServices.RemoveImagesFromApi(images);
            _dbContext.Kindergartens.Remove(result);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Debug.WriteLine("Exception:  " + e.Message);
        }

        return result;
    }
}