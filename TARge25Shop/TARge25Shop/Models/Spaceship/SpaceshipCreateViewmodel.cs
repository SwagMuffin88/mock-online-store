using System.ComponentModel.DataAnnotations;

namespace TARge25Shop.Models.Spaceship;

public class SpaceshipCreateViewModel
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public string ShipType { get; set; } = string.Empty;
    public int MaxCrewSize { get; set; }
    public int EnginePower { get; set; }
    public List<IFormFile> Files { get; set; }
}