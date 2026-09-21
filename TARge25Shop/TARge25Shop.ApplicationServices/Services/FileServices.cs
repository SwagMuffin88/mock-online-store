using TARge25Shop.Core;
using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;
using TARge25Shop.Data;
using Microsoft.Extensions.Hosting;

namespace TARge25Shop.ApplicationServices;

public class FileServices :IFileServices
{
    private readonly TARge25ShopContext _dbContext;

    private  readonly IHostEnvironment _webHost;
    
    public FileServices(TARge25ShopContext dbContext, IHostEnvironment webHost)
    {
        _dbContext = dbContext;
        _webHost = webHost;
    }

    public void ConvertFilesToApi(SpaceshipDto dto, Spaceship spaceship)
    {
        string ContentRootPath = _webHost.ContentRootPath;
        string _path = ContentRootPath + "\\wwroot\\multipleFileUpload\\";
        
        if (dto.Files != null && dto.Files.Count >= 0)
        {
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }

            foreach (var file in dto.Files)
            {
                string uploadsFolder = Path.Combine(ContentRootPath, "wwroot", "multipleFileUpload");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string fullPath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                    FileToApi path = new FileToApi
                    {
                        Id = Guid.NewGuid(),
                        ExistingFilePath = uniqueFileName,
                        SpaceshipId = spaceship.Id
                    };
                }
            }
        }
    }
}