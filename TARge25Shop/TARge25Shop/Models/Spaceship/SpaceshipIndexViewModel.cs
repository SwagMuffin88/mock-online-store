using System.ComponentModel.DataAnnotations;
using TARge25Shop.Core.Dto;

namespace TARge25Shop.Models.Spaceship;

public class SpaceshipIndexViewModel 
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public string ShipType { get; set; } = string.Empty;
    public int MaxCrewSize { get; set; }
    public int EnginePower { get; set; }
    
    public List<IFormFile> Files { get; set; }
    public List<ImageViewModel> Images { get; set; } = new List<ImageViewModel>();

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}