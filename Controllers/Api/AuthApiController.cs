
using AcxiomCRM.Models;
using AcxiomCRM.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AcxiomCRM.Controllers.Api;

[ApiController]
[Route("api/auth")]
public class AuthApiController(
    UserManager<ApplicationUser> users,
    SignInManager<ApplicationUser> signIn,
    AuditService audit) : ControllerBase
{
    [HttpPost("login")]
    [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting("auth")]
    public async Task<IActionResult> Login(ApiLoginDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var user = await users.FindByEmailAsync(dto.Email);

        if (user == null || !user.IsActive)
        {
            await audit.WriteAsync(
                null, "Authentication", "ApiLoginFailed",
                null, dto.Email, "Failed");

            return Unauthorized(new
            {
                error = "Invalid credentials or inactive account."
            });
        }

        var result = await signIn.PasswordSignInAsync(
            user, dto.Password, dto.RememberMe, true);

        if (!result.Succeeded)
        {
            await audit.WriteAsync(
                null, "Authentication",
                result.IsLockedOut ? "Lockout" : "ApiLoginFailed",
                null, dto.Email, "Failed");

            return Unauthorized(new
            {
                error = result.IsLockedOut
                    ? "Account temporarily locked."
                    : "Invalid credentials."
            });
        }

        await audit.WriteAsync(
            User, "Authentication", "ApiLoginSuccess");

        return Ok(new
        {
            message = "Signed in.",
            user = new
            {
                id = user.Id,
                name = user.FullName,
                email = user.Email,
                roles = await users.GetRolesAsync(user)
            }
        });
    }

    [HttpPost("logout")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public async Task<IActionResult> Logout()
    {
        await audit.WriteAsync(
            User, "Authentication", "ApiLogout");

        await signIn.SignOutAsync();

        return Ok(new { message = "Signed out." });
    }
}

public record ApiLoginDto(
    [Required, EmailAddress] string Email,
    [Required] string Password,
    bool RememberMe = false);
