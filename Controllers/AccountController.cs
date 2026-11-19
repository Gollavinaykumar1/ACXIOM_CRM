using Microsoft.AspNetCore.Authorization;
using AcxiomCRM.Models;
using AcxiomCRM.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AcxiomCRM.Controllers;

public class AccountController(
    UserManager<ApplicationUser> users,
    SignInManager<ApplicationUser> signIn,
    AuditService audit) : Controller
{
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
        => View(new LoginVm { ReturnUrl = returnUrl });

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVm m)
    {
        if (!ModelState.IsValid) return View(m);

        var u = await users.FindByEmailAsync(m.Email);

        if (u == null || !u.IsActive)
        {
            ModelState.AddModelError("", "Invalid email or password, or account inactive.");
            await audit.WriteAsync(null, "Authentication", "LoginFailed", null, m.Email, "Failed");
            return View(m);
        }

        var r = await signIn.PasswordSignInAsync(u, m.Password, m.RememberMe, true);

        if (r.Succeeded)
        {
            await audit.WriteAsync(User, "Authentication", "LoginSuccess");
            return LocalRedirect(m.ReturnUrl ?? Url.Action("Index", "Dashboard")!);
        }

        if (r.IsLockedOut)
            ModelState.AddModelError("", "Account locked temporarily after repeated failed attempts.");
        else
            ModelState.AddModelError("", "Invalid email or password.");

        await audit.WriteAsync(null, "Authentication",
            r.IsLockedOut ? "Lockout" : "LoginFailed",
            null, m.Email, "Failed");

        return View(m);
    }

    [HttpGet]
    public IActionResult Register() => View(new RegisterVm());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterVm m)
    {
        if (!ModelState.IsValid) return View(m);

        var u = new ApplicationUser
        {
            UserName = m.Email,
            Email = m.Email,
            FullName = m.FullName,
            IsActive = true
        };

        var r = await users.CreateAsync(u, m.Password);

        if (r.Succeeded)
        {
            await users.AddToRoleAsync(u, "Sales Executive");
            await audit.WriteAsync(null, "Authentication", "Registration", u.Id);
            TempData["Success"] = "Account created. Please sign in.";
            return RedirectToAction(nameof(Login));
        }

        foreach (var e in r.Errors)
            ModelState.AddModelError("", e.Description);

        return View(m);
    }

    [HttpPost, Authorize, ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await audit.WriteAsync(User, "Authentication", "Logout");
        await signIn.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }

    public IActionResult Denied() => View();
}

public class LoginVm
{
    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = "";

    public bool RememberMe { get; set; }
    public string? ReturnUrl { get; set; }
}

public class RegisterVm
{
    [Required, StringLength(100)]
    public string FullName { get; set; } = "";

    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Required, MinLength(8), DataType(DataType.Password)]
    public string Password { get; set; } = "";

    [Compare(nameof(Password)), DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = "";
}