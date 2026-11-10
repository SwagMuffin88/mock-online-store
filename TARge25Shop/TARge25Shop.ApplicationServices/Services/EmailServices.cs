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

    private EmailServices(IConfiguration config)
    {
        _config = config;
    }
    
    public void SendEmail(EmailDto dto)
    {
        var from = MailboxAddress.Parse(_config.GetSection("EmailSettings.SenderName").Value);
        var to =  new MailboxAddress(
            SplitEmail(dto.To),
            dto.To);
        
        var email = new MimeMessage();
        
        email.From.Add(from);
        email.To.Add(to);

        var builder = new BodyBuilder
        {
            HtmlBody = dto.Body
        };

        foreach (var file in dto.Attachments)
        {
            if (file.Length > 0 && file.Length < 10485760)
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    stream.Position = 0; // Resets the stream position to the beginning
                    builder.Attachments.Add(file.FileName,  stream.ToArray());
                }
            }
        }
        
        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        
        smtp.Connect(_config.GetSection("EmailSettings.Server").Value,  
            587,
            MailKit.Security.SecureSocketOptions.StartTls
        );
        
        smtp.Authenticate(_config.GetSection("EmailSettings.Email").Value,
            _config.GetSection("EmailSettings.Password").Value);
        
        smtp.Send(email);
        smtp.Disconnect(true);
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