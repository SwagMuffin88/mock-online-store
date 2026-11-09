using Microsoft.Extensions.Configuration;
using MimeKit;
using TARge25Shop.Core.Dto.email;
using TARge25Shop.Core.ServiceInterface;

namespace TARge25Shop.ApplicationServices.Services;

public class EmailServices : IEmailServices 
{

    private readonly IConfiguration _config;

    private EmailServices(IConfiguration config)
    {
        _config = config;
    }
    
    public void SendEmail(EmailDto dto)
    {
        var from = MailboxAddress.Parse(_config.GetSection("EmailUserName").Value);
        var to =  new MailboxAddress(
            SplitEmail(dto.To),
            dto.To);
        
        var email = new MimeMessage();
        
        email.From.Add(from);
        email.To.Add(to);
    }

    private string SplitEmail(string email)
    {
        int atSymbolIndex = email.IndexOf('@');
        string leftHalf = string.Empty;
        if (atSymbolIndex != -1)
        {
            leftHalf = email.Substring(0, atSymbolIndex);
        }

        return leftHalf;
    }
}