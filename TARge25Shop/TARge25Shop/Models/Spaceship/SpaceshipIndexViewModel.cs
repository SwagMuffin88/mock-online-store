using TARge25Shop.Core.Dto;

namespace TARge25Shop.Models.Spaceship;

public class SpaceshipIndexViewModel
{
    public Guid? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ShipType { get; set; } = string.Empty;
    public int MaxCrewSize { get; set; }
    public int EnginePower { get; set; }
    
    public List<IFormFile> Files { get; set; }
    public List<ImageViewModel> Images { get; set; } = new List<ImageViewModel>();
    // public IEnumerable<FileToApiDto> FileToApiDtos { get; set; }
    //     = new List<FileToApiDto>();

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}