using Microsoft.AspNetCore.Mvc;
using MailKit;
using TARge25Shop.Core.Dto.email;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Models.Email;

namespace TARge25Shop.Controllers;

public class EmailController : Controller
{
    private readonly IConfiguration _config;
    private readonly IEmailServices _services;
    
    public EmailController(IConfiguration config,  IEmailServices services)
    {
        _config = config;
        _services = services;
    }

    public IActionResult Index()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Send(EmailViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", viewModel);
        }
        
        try
        {
            var dto = new EmailDto
            {
                To = viewModel.To,
                Subject = viewModel.Subject,
                Body = viewModel.Body,
                Attachments = new List<EmailAttachmentDto>()
            };

            if (viewModel.Attachments != null && viewModel.Attachments.Any())
            {
                foreach (var file in viewModel.Attachments)
                {
                    if (file.Length > 0 && file.Length < 10485760)
                    {
                        using (var stream = new MemoryStream())
                        {
                            file.CopyTo(stream);
                            
                            dto.Attachments.Add(new EmailAttachmentDto
                            {
                                FileName = file.FileName,
                                Data = stream.ToArray()
                            });
                        }
                    }
                }
            }
            
            _services.SendEmail(dto);
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            ModelState.AddModelError(string.Empty, $"Sending email failed: {e.Message}");
            System.Diagnostics.Debug.Print(e.Message);
            return View("Index", viewModel);
        }
    }
}