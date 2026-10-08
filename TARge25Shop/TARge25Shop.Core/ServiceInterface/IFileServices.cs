using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;

namespace TARge25Shop.Core;

public interface IFileServices
{
    public void UploadFilesToApi(SpaceshipDto dto, Spaceship spaceship);
    public void UploadFilesToApi(KindergartenDto dto, Kindergarten kindergarten);
    
    
    Task<bool> RemoveImageFromApi(FileToApiDto dto, bool saveChanges = true);
    Task<bool> RemoveImagesFromApi(FileToApiDto[] dtos);
    
}