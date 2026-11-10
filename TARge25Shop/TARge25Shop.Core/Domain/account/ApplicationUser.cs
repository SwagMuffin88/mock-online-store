using Microsoft.AspNetCore.Identity;

namespace TARge25Shop.Core.Dto.account;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
}