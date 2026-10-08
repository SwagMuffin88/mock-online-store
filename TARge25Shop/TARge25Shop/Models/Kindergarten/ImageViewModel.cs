namespace TARge25Shop.Models.Kindergarten;

public class ImageViewModel
{
    public string FilePath { get; set; } =  String.Empty;
    public Guid ImageId { get; set; }
    public Guid? KindergartenId { get; set; }
}