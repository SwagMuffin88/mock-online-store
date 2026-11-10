using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using TARge25Shop.ApplicationServices.Customs;
using TARge25Shop.Core.Dto.email;
using TARge25Shop.Core.ServiceInterface;

namespace TARge25Shop.ApplicationServices.Services;

public class EmailServices : IEmailServices 
{

    private readonly IConfiguration _config;
    private readonly EmailSettings _emailSettings;
    private readonly SmtpClient _smtpClient;

    private EmailServices(IConfiguration config, IOptions<EmailSettings> emailSettings)
    {
        _config = config;
        _emailSettings =  emailSettings.Value;
        
        
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