using Microsoft.AspNetCore.Http;

namespace TARge25Shop.Core.Dto.email;

public class EmailDto
{
    public string From { get; set; } = string.Empty;
    public string To { get; set;} = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public List<IFormFile> Attachments { get; set; } = new List<IFormFile>();
}