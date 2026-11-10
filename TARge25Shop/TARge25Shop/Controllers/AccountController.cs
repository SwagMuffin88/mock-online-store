using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TARge25Shop.ApplicationServices.Services;
using TARge25Shop.Core.Dto.account;
using TARge25Shop.Core.Dto.email;
using TARge25Shop.Models.Account;

namespace TARge25Shop.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly EmailServices _emailServices;

    public AccountController(ILogger<AccountController> logger, UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, EmailServices emailServices)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _emailServices =  emailServices;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var user = new ApplicationUser
                {
                    UserName = model.Name,
                    Name =  model.Name,
                    Email = model.Email,
                    City = model.City
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    
                    var confirmationLink = Url.Action(
                        "ConfirmEmail", 
                        "Account", 
                        new { userId = user.Id, token = token }, 
                        Request.Scheme);

                    EmailTokenDto newsignup = new EmailTokenDto
                    {
                        Token = token,
                        Body = $"Please confirm your account by clicking this link: <a href='{confirmationLink}'>Confirm Email</a>",
                        Subject = "SHOP Confirm your email",
                        To = user.Email
                    };

                    _emailServices.SendEmailToken(newsignup, token);

                    List<string> errorDataList =
                    [
                        "Area", "Accounts",
                        "Issue", "Success",
                        "StatusMessage", "Registration success",
                        "ActedOn", $"{model.Email}",
                        "CreatedAccountData", $"{model.Email}\n{model.City}\n[Password hidden]\n[Password hidden]"
                    ];
                    ViewBag.ErrorData = errorDataList;
                    ViewBag.ErrorTitle = "Registration success";
                    ViewBag.ErrorMessage = "Check your email to confirm your account.";
                    
                    return View("ConfirmationEmailMessage");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Print(e.Message);
            }
        }
        return View(model);
    }
}