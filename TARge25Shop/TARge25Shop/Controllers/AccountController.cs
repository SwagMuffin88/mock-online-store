using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TARge25Shop.ApplicationServices.Services;
using TARge25Shop.Core.Domain.Account;
using TARge25Shop.Core.Dto.email;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Models;
using TARge25Shop.Models.Account;

namespace TARge25Shop.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailServices _emailServices;

    public AccountController(ILogger<AccountController> logger, UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, IEmailServices emailServices)
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
    public async Task<IActionResult> Register(RegisterViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);
        
        var user = new ApplicationUser
        {
            UserName = viewModel.Name,
            Name =  viewModel.Name,
            Email = viewModel.Email,
            City = viewModel.City
        };

        var result = await _userManager.CreateAsync(user, viewModel.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(viewModel);
        }

        try {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            
            var confirmationLink = Url.Action(
                "ConfirmEmail", 
                "Account", 
                new { userId = user.Id, token = token }, 
                Request.Scheme);
            
            if (string.IsNullOrWhiteSpace(confirmationLink))
            {
                throw new InvalidOperationException("Could not generate confirmation link.");
            }
            
            var newSignup = new EmailTokenDto
            {
                Token = token,
                Body = $"Please confirm your account by clicking this link: <a href='{confirmationLink}'>Confirm Email</a>",
                Subject = "Confirm your email",
                To = user.Email
            };

            _emailServices.SendEmailToken(newSignup, token);
            
            ViewBag.StatusTitle = "Registration success";
            ViewBag.StatusMessage = "Check your email to confirm your account.";
            
            return View("ConfirmationEmailMessage");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not send confirmation link to email {Email}", user.Email);
            ModelState.AddModelError(
                string.Empty, "Account created, but there was a problem sending the confirmation link. Try again later.");
            return View(viewModel);
        }
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
        {
            return RedirectToAction("Index", "Home");
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            ViewBag.ErrorTitle = "User not found";
            ViewBag.ErrorMessage = "The specified user account does not exist.";
            return View("NotFound");
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);
        
        if (result.Succeeded)
        {
            ViewBag.StatusTitle = "Email confirmed";
            ViewBag.StatusMessage = "Your email has been confirmed. You can now log in.";
            return View();
        }
        
        ViewBag.ErrorTitle = "Email cannot be confirmed";
        ViewBag.ErrorMessage = "The email confirmation link is invalid or has expired.";

        return View("Error", new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string? returnUrl)
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel viewModel, string? returnUrl)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(viewModel.Email);

            if (user != null && !user.EmailConfirmed &&
                (await _userManager.CheckPasswordAsync(user, viewModel.Password)))
            {
                ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                return View(viewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.RememberMe, true);
                
            if(result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                
                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                return View("AccountLocked");
            }

            ModelState.AddModelError("", "Invalid login Attempt");
        }

        return View(viewModel);
    }
    
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(viewModel.Email);
            if (user != null && await _userManager.IsEmailConfirmedAsync(user))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                
                var passwordResetLink = Url.Action("ResetPassword", "Account", new { email = viewModel.Email, token = token }, Request.Scheme);

                var emailDto = new EmailDto
                {
                    To = viewModel.Email,
                    Subject = "reset your password",
                    Body = $"Please reset your password by clicking <a href='{passwordResetLink}'>here</a>"
                };

                _emailServices.SendEmail(emailDto);
                
                return View("ForgotPasswordConfirmation");
            }
        }
        return View(viewModel);
    }
        
    [HttpGet]
    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult ResetPassword(string email, string token)
    {
        if (token == null || email == null)
        {
            ModelState.AddModelError("", "Could not initiate password reset");
        }

        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        var user = await _userManager.FindByEmailAsync(viewModel.Email);

        // Ära reeda, kas sellise emailiga kasutaja on olemas
        if (user == null)
            return View("ResetPasswordConfirmation");

        var result = await _userManager.ResetPasswordAsync(user, viewModel.Token, viewModel.Password);

        if (result.Succeeded)
        {
            //SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow) töötab,
            // kuid null väärtus eemaldab lukustuse täielikult (LockoutEnd = null).
            if (await _userManager.IsLockedOutAsync(user)) 
                //await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                await _userManager.SetLockoutEndDateAsync(user, null);
            
            return View("ResetPasswordConfirmation");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View(viewModel);
    }

    [HttpGet]
    public IActionResult ChangePassword(string email, string token)
    {
        if (token == null || email == null)
        {
            ModelState.AddModelError("", "Could not initiate password reset");
        }
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);
        
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return RedirectToAction("Login", "Account");

        bool passwordsMatch = await _userManager.CheckPasswordAsync(user, viewModel.NewPassword);

        if (passwordsMatch)
        {
            ModelState.AddModelError(string.Empty, "New password must be different from the old password.");
            return View(viewModel);
        }

        var result = await _userManager.ChangePasswordAsync(user, viewModel.CurrentPassword, viewModel.NewPassword);
        
        if (result.Succeeded) {
            await _signInManager.RefreshSignInAsync(user);
            return View("ChangePasswordConfirmation");
        }
        
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View(viewModel);
    }
}