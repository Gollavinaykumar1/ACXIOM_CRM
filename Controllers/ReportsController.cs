using AcxiomCRM.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AcxiomCRM.Controllers;

public class PipelineReportRow
{
    public string Stage { get; set; } = "";
    public decimal Amount { get; set; }
    public decimal Weighted { get; set; }
}

[Authorize]
public class ReportsController(ApplicationDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        var query = db.Opportunities.AsQueryable();

        if (!User.IsInRole("Admin") && !User.IsInRole("Manager"))
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            query = query.Where(x => x.OwnerId == userId);
        }

        var rows = await query.ToListAsync();

        var pipeline = rows.GroupBy(x => x.Stage)
            .Select(g => new PipelineReportRow
            {
                Stage = g.Key,
                Amount = g.Sum(x => x.Amount),
                Weighted = g.Sum(x => x.Amount * x.Probability / 100m)
            }).ToList();

        return View(pipeline);
    }

    public async Task<IActionResult> ExportCustomers()
    {
        var query = db.Customers.AsQueryable();

        if (!User.IsInRole("Admin") && !User.IsInRole("Manager"))
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            query = query.Where(x => x.OwnerId == userId);
        }

        var rows = await query.ToListAsync();

        var csv = "Id,Name,Email,Phone,Status\n" +
            string.Join("\n", rows.Select(x =>
                $"{x.Id},\"{x.Name}\",\"{x.Email}\",\"{x.Phone}\",\"{x.Status}\""));

        return File(System.Text.Encoding.UTF8.GetBytes(csv),
            "text/csv", "customers.csv");
    }
}