using System.ComponentModel.DataAnnotations;

namespace TARge25Shop.Models.Spaceship;

public class SpaceshipUpdateViewmodel : SpaceshipFormViewModel
{
    [Required]
    public Guid Id { get; set; }
    public List<ImageViewModel> Images { get; set; } = new List<ImageViewModel>();

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}