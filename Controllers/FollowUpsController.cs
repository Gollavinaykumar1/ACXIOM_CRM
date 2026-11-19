using AcxiomCRM.Data;using AcxiomCRM.Models;using AcxiomCRM.Services;using Microsoft.AspNetCore.Authorization;using Microsoft.AspNetCore.Mvc;using Microsoft.EntityFrameworkCore;
namespace AcxiomCRM.Controllers;
[Authorize] public class FollowUpsController(ApplicationDbContext db,AuditService audit):Controller{
 public async Task<IActionResult>Index(){var q=db.FollowUps.Include(x=>x.AssignedUser).AsQueryable();if(!User.IsInRole("Admin")&&!User.IsInRole("Manager"))q=q.Where(x=>x.AssignedUserId==User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);return View(await q.OrderBy(x=>x.FollowUpDate).ToListAsync());}
 public IActionResult Create()=>View(new FollowUp());
 [HttpPost,ValidateAntiForgeryToken]public async Task<IActionResult>Create(FollowUp m){if(m.FollowUpDate.Date<DateTime.Today)ModelState.AddModelError(nameof(m.FollowUpDate),"Follow-up date cannot be earlier than today.");
 var uid=User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value;
 if(m.RelatedId.HasValue&&m.RelatedType=="Customer"){var related=await db.Customers.FirstOrDefaultAsync(c=>c.Id==m.RelatedId.Value);if(related==null||(!User.IsInRole("Admin")&&!User.IsInRole("Manager")&&related.OwnerId!=uid))ModelState.AddModelError(nameof(m.RelatedId),"Select a customer you are allowed to access.");}
 else if(m.RelatedId.HasValue&&m.RelatedType=="Lead"){var related=await db.Leads.FirstOrDefaultAsync(l=>l.Id==m.RelatedId.Value);if(related==null||(!User.IsInRole("Admin")&&!User.IsInRole("Manager")&&related.OwnerId!=uid))ModelState.AddModelError(nameof(m.RelatedId),"Select a lead you are allowed to access.");}
 else if(m.RelatedType!="Customer"&&m.RelatedType!="Lead")ModelState.AddModelError(nameof(m.RelatedType),"Related type must be Customer or Lead.");
 if(!ModelState.IsValid)return View(m);m.AssignedUserId=uid;db.FollowUps.Add(m);await db.SaveChangesAsync();await audit.WriteAsync(User,"FollowUps","Create",m.Id.ToString(),m.Subject);return RedirectToAction(nameof(Index));}
 [HttpPost,ValidateAntiForgeryToken]public async Task<IActionResult>Status(int id,string status){var x=await db.FollowUps.FindAsync(id);if(x==null)return NotFound();if(!User.IsInRole("Admin")&&!User.IsInRole("Manager")&&x.AssignedUserId!=User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value)return Forbid();if(!new[]{"Planned","Completed","Missed","Cancelled"}.Contains(status))return BadRequest();x.Status=status;await db.SaveChangesAsync();await audit.WriteAsync(User,"FollowUps","StatusChanged",id.ToString(),status);return RedirectToAction(nameof(Index));}
}
