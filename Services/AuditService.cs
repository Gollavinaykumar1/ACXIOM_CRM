using AcxiomCRM.Data; using AcxiomCRM.Models; using System.Security.Claims;
namespace AcxiomCRM.Services;
public class AuditService(ApplicationDbContext db) {
 public async Task WriteAsync(ClaimsPrincipal? principal,string module,string action,string? recordId=null,string? details=null,string result="Success") {
  var userId=principal?.FindFirstValue(ClaimTypes.NameIdentifier); var name=principal?.Identity?.Name ?? "System";
  db.AuditLogs.Add(new AuditLog{UserId=userId,UserName=name,Module=module,Action=action,RecordId=recordId,Details=details,Result=result}); await db.SaveChangesAsync();
 }
}
