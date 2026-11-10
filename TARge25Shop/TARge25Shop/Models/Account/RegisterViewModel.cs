using System.ComponentModel.DataAnnotations;

namespace TARge25Shop.Models.Account;

public class RegisterViewModel
{
    [Required]
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    
    [DataType(DataType.Password)]
    [Required]
    public string Password { get; set; } = string.Empty;
    
    [DataType(DataType.Password)]
    [Required]
    public string ConfirmPassword { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
}