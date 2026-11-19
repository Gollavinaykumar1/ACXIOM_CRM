using AcxiomCRM.Models;using AcxiomCRM.Services;using Microsoft.AspNetCore.Authorization;using Microsoft.AspNetCore.Identity;using Microsoft.AspNetCore.Mvc;using Microsoft.EntityFrameworkCore;
namespace AcxiomCRM.Controllers;
[Authorize(Roles="Admin")] public class UsersController(UserManager<ApplicationUser> users,AuditService audit):Controller{
 public async Task<IActionResult>Index()=>View(await users.Users.OrderBy(x=>x.FullName).ToListAsync());
 [HttpPost,ValidateAntiForgeryToken]public async Task<IActionResult>Toggle(string id){var u=await users.FindByIdAsync(id);if(u==null)return NotFound();if(u.Id==users.GetUserId(User))return BadRequest("You cannot deactivate your own account.");u.IsActive=!u.IsActive;await users.UpdateAsync(u);await audit.WriteAsync(User,"Users",u.IsActive?"Activate":"Deactivate",id,u.Email);return RedirectToAction(nameof(Index));}
}
