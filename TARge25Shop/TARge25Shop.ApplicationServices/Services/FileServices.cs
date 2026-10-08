using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TARge25Shop.Core;
using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;
using TARge25Shop.Data;
using Microsoft.Extensions.Hosting;

namespace TARge25Shop.ApplicationServices.Services;

public class FileServices :IFileServices
{
    private readonly TARge25ShopContext _dbContext;
    private  readonly IHostEnvironment _webHost;
    
    public FileServices(TARge25ShopContext dbContext, IHostEnvironment webHost)
    {
        _dbContext = dbContext;
        _webHost = webHost;
    }

    public void UploadFilesToApi(SpaceshipDto dto, Spaceship spaceship)
    {
        UploadFilesToApiHelper(dto.Files, spaceship.Id);
    }

    public void UploadFilesToApi(KindergartenDto dto, Kindergarten kindergarten)
    {
        UploadFilesToApiHelper(dto.Files, kindergarten.Id);
    }

    private void UploadFilesToApiHelper(List<IFormFile> files, Guid objectId)
    {
        if (files == null || files.Count == 0)
        {
            return;
        }
        string uploadsFolder = Path.Combine(_webHost.ContentRootPath, "wwwroot", "multipleFileUpload");

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        foreach (var file in files)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string fullPath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(fileStream);

                FileToApi path = new FileToApi
                {
                    Id = Guid.NewGuid(),
                    ExistingFilePath = uniqueFileName,
                    ObjectId = objectId
                };

                _dbContext.FilesToApis.Add(path);
            }
        }
    }

    public async Task<bool> RemoveImageFromApi(FileToApiDto dto, bool saveChanges = true)
    {
        var image = await _dbContext.FilesToApis
            .FirstOrDefaultAsync(x => x.Id == dto.Id);
        
        if (image == null) return false;
        
        var filePath = Path.Combine(_webHost.ContentRootPath, "wwwroot", "multipleFileUpload", image.ExistingFilePath);
        
        if (File.Exists(filePath)) File.Delete(filePath);

        _dbContext.FilesToApis.Remove(image);
        
        if (saveChanges)
        {
            await _dbContext.SaveChangesAsync();
        }
        return true;
    }

    public async Task<bool> RemoveImagesFromApi(FileToApiDto[] dtos)
    {
        foreach (var dto in dtos)
        {
            await RemoveImageFromApi(dto, false);    // Separate bool for avoiding making multiple calls to db
        }
        await _dbContext.SaveChangesAsync();
        
        return true;
    }
}