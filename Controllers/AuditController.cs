using AcxiomCRM.Data;using Microsoft.AspNetCore.Authorization;using Microsoft.AspNetCore.Mvc;using Microsoft.EntityFrameworkCore;
namespace AcxiomCRM.Controllers;
[Authorize(Roles="Admin,Manager")] public class AuditController(ApplicationDbContext db):Controller{public async Task<IActionResult>Index(string? module)=>View(await db.AuditLogs.Where(x=>module==null||x.Module==module).OrderByDescending(x=>x.Timestamp).Take(500).ToListAsync());}
