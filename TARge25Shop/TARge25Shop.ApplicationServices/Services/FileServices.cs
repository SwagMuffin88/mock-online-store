using TARge25Shop.Core;
using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;
using TARge25Shop.Data;

namespace TARge25Shop.ApplicationServices;

public class FileServices :IFileServices
{
    private readonly TARge25ShopContext _dbContext;

    public FileServices(TARge25ShopContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void FilesToApi(SpaceshipDto dto, Spaceship spaceship)
    {
        throw new NotImplementedException();
    }
}