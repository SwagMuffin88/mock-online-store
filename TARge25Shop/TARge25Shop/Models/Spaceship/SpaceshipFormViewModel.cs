namespace TARge25Shop.Models.Spaceship;

public abstract class SpaceshipFormViewModel
{
    public string Name { get; set; } = String.Empty;
    public string ShipType { get; set; } = String.Empty;
    public int MaxCrewSize { get; set; }
    public int EnginePower { get; set; }
    public List<IFormFile> Files { get; set; }
}