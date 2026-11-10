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

    public EmailServices(IConfiguration config)
    {
        _config = config;
    }
    
    public void SendEmail(EmailDto dto)
    {
        var senderName = _config["EmailSettings:SenderName"] ?? "TARge25Shop";
        var server = _config["EmailSettings:Server"];
        var emailUser = _config["EmailSettings:Email"];
        var password = _config["EmailSettings:Password"];
        
        if (string.IsNullOrEmpty(emailUser))
        {
            throw new Exception("Could not get sender value from configuration");
        }

        if (string.IsNullOrEmpty(dto.To))
        {
            throw new Exception("Recipient value cannot be empty");
        }
        
        var from = new MailboxAddress(senderName, emailUser);
        var to =  new MailboxAddress(SplitEmail(dto.To), dto.To);
        
        var email = new MimeMessage();
        
        email.From.Add(from);
        email.To.Add(to);
        email.Subject = dto.Subject;

        var builder = new BodyBuilder
        {
            HtmlBody = dto.Body
        };

        if (dto.Attachments != null) 
        {
            foreach (var file in dto.Attachments)
            {
                    using (var stream = new MemoryStream())
                    {
                        stream.Position = 0; // Resets the stream position to the beginning
                        builder.Attachments.Add(file.FileName,  stream.ToArray());
                    }
            }
        }
        
        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        
        smtp.Connect(server,  
            587,
            MailKit.Security.SecureSocketOptions.StartTls
        );
        
        smtp.Authenticate(emailUser,
            password);
        
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