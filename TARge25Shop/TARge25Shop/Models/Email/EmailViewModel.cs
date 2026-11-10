using System.ComponentModel.DataAnnotations;

namespace TARge25Shop.Models.Email;

public class EmailViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is invalid")]
    public string From { get; set; } = string.Empty;
    
    public string To { get; set;} = string.Empty;
    
    public string Subject { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Email body is required")]
    public string Body { get; set; } = string.Empty;
    
    public List<IFormFile> Attachments { get; set; } = new();
}