using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace AcxiomCRM.Models;
public class ApplicationUser : IdentityUser {
 [Required, StringLength(100)] public string FullName {get;set;} = "";
 public bool IsActive {get;set;} = true;
 public ICollection<Customer> Customers {get;set;} = new List<Customer>();
}
public class Customer {
 public int Id {get;set;} [Required,StringLength(120)] public string Name {get;set;}="";
 [EmailAddress,StringLength(160)] public string? Email {get;set;}
 [Phone,StringLength(20)] public string? Phone {get;set;}
 [StringLength(300)] public string? Address {get;set;}
 [StringLength(30)] public string Status {get;set;}="Active";
 public string? OwnerId {get;set;} public ApplicationUser? Owner {get;set;}
 public DateTime CreatedAt {get;set;}=DateTime.UtcNow; public DateTime? UpdatedAt {get;set;}
 public ICollection<Opportunity> Opportunities {get;set;}=new List<Opportunity>();
}
public class Lead {
 public int Id {get;set;} [Required,StringLength(120)] public string Name {get;set;}="";
 [EmailAddress] public string? Email {get;set;} [Phone] public string? Phone {get;set;}
 [StringLength(60)] public string Source {get;set;}="Website";
 [StringLength(30)] public string Status {get;set;}="New";
 [StringLength(20)] public string Priority {get;set;}="Normal";
 public string? OwnerId {get;set;} public ApplicationUser? Owner {get;set;}
 public DateTime CreatedAt {get;set;}=DateTime.UtcNow; public DateTime? ConvertedAt {get;set;}
 [StringLength(1000)] public string? Notes {get;set;}
}
public class Opportunity {
 public int Id {get;set;} [Required,StringLength(140)] public string Name {get;set;}="";
 public int CustomerId {get;set;} public Customer? Customer {get;set;}
 [StringLength(30)] public string Stage {get;set;}="Qualification";
 [Range(0.01,1000000000)] public decimal Amount {get;set;}
 [Range(0,100)] public int Probability {get;set;}=20;
 [DataType(DataType.Date)] public DateTime ExpectedCloseDate {get;set;}=DateTime.Today.AddDays(30);
 public string? OwnerId {get;set;} public ApplicationUser? Owner {get;set;}
 [StringLength(1000)] public string? Notes {get;set;}
 public decimal WeightedValue => Amount*Probability/100m;
}
public class FollowUp {
 public int Id {get;set;} [Required,StringLength(140)] public string Subject {get;set;}="";
 [Required,DataType(DataType.Date)] public DateTime FollowUpDate {get;set;}=DateTime.Today;
 [StringLength(30)] public string Status {get;set;}="Planned";
 [StringLength(40)] public string RelatedType {get;set;}="Customer";
 public int? RelatedId {get;set;} public string? AssignedUserId {get;set;} public ApplicationUser? AssignedUser {get;set;}
 [StringLength(1000)] public string? Notes {get;set;}
}
public class AuditLog {
 public long Id {get;set;} public string? UserId {get;set;} [StringLength(120)] public string UserName {get;set;}="System";
 [Required,StringLength(40)] public string Module {get;set;}="";
 [Required,StringLength(60)] public string Action {get;set;}="";
 public string? RecordId {get;set;} public DateTime Timestamp {get;set;}=DateTime.UtcNow;
 [StringLength(20)] public string Result {get;set;}="Success"; [StringLength(1000)] public string? Details {get;set;}
}
