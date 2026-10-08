using System.ComponentModel.DataAnnotations;

namespace TARge25Shop.Models.Kindergarten;

public class KindergartenUpdateViewmodel : KindergartenFormViewModel
{
    //[Required]
    public Guid Id { get; set; }
    public List<ImageViewModel>? Images { get; set; } = new List<ImageViewModel>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}