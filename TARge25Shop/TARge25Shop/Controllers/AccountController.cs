using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TARge25Shop.ApplicationServices.Services;
using TARge25Shop.Core.Dto.account;
using TARge25Shop.Core.Dto.email;
using TARge25Shop.Models;
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

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (userId == null || token == null)
        {
            return RedirectToAction("Index", "Home");
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            ViewBag.ErrorMessage = $"The user with id of {userId} is not valid";
            return View("NotFound");
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);
        List<string> errorData =
            [
                "Area", "Account",
                "Issue", "Success",
                "StatusMessage", "Registration Success",
                "ActedOn", $"{user.Email}",
                "CreatedAccountData", $"{user.Email}\n{user.City}\n[password hidden]"
            ];
        
        if (result.Succeeded)
        {
            errorData =
            [
                "Area", "Account",
                "Issue", "Success",
                "StatusMessage", "Registration Success",
                "ActedOn", $"{user.Email}",
                "CreatedAccountData", $"{user.Email}\n{user.City}\n[password hidden]"
            ];
            ViewBag.ErrorMessage = errorData;
            return View();
        }

        ViewBag.ErrorMessage = errorData;
        ViewBag.ErrorTitles = "Email cannot be confirmed";
        ViewBag.ErrorMessage = $"The user email, with userid of {userId}, cannot be confirmed";

        return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string? returnUrl)
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && !user.EmailConfirmed &&
                (await _userManager.CheckPasswordAsync(user, model.Password)))
            {
                ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);
                
            if(result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            if (result.IsLockedOut)
            {
                return View("AccountLocked");
            }

            ModelState.AddModelError("", "Invalid login Attempt");
        }

        return View(model);
    }
    
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.IsEmailConfirmedAsync(user))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                
                var passwordResetLink = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);

                var emailDto = new EmailDto
                {
                    To = model.Email,
                    Subject = "reset your password",
                    Body = $"Please reset your password by clicking <a href='{passwordResetLink}'>here</a>"
                };

                _emailServices.SendEmail(emailDto);

                return View("ForgotPasswordConfirmation");
            }
            return View("ForgotPasswordConfirmation");
        }
        return View(model);
    }

}