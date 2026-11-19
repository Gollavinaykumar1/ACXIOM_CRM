using AcxiomCRM.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace AcxiomCRM.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {
 public DbSet<Customer> Customers=>Set<Customer>(); public DbSet<Lead> Leads=>Set<Lead>();
 public DbSet<Opportunity> Opportunities=>Set<Opportunity>(); public DbSet<FollowUp> FollowUps=>Set<FollowUp>(); public DbSet<AuditLog> AuditLogs=>Set<AuditLog>();
 protected override void OnModelCreating(ModelBuilder b) { base.OnModelCreating(b); b.Entity<Customer>().HasIndex(x=>x.Email); b.Entity<Lead>().HasIndex(x=>x.Status); b.Entity<Opportunity>().HasIndex(x=>x.Stage); b.Entity<AuditLog>().HasIndex(x=>x.Timestamp); b.Entity<Opportunity>().Property(x=>x.Amount).HasPrecision(18,2); }
}
