namespace TARge25Shop.Models.Kindergarten;

public abstract class KindergartenFormViewModel
{
    public string GroupName { get; set; } = string.Empty;
    public int ChildrenCount { get; set; }
    public string KindergartenName { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;
    
    public List<IFormFile> Files { get; set; }
}