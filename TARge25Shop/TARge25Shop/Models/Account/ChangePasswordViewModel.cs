using System.ComponentModel.DataAnnotations;

namespace TARge25Shop.Models.Account;

public class ChangePasswordViewModel
{
    [EmailAddress] 
    public string Email { get; set; } = string.Empty;
    
    public string Token { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = string.Empty;
    
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "Password and confirmation password do not match.")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}