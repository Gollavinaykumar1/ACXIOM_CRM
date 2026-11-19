using AcxiomCRM.Models; using Microsoft.AspNetCore.Identity;
namespace AcxiomCRM.Data;
public static class SeedData {
 public static async Task Initialize(IServiceProvider sp) {
  var roles=sp.GetRequiredService<RoleManager<IdentityRole>>(); var users=sp.GetRequiredService<UserManager<ApplicationUser>>();
  foreach(var r in new[]{"Admin","Manager","Sales Executive"}) if(!await roles.RoleExistsAsync(r)) await roles.CreateAsync(new IdentityRole(r));
  async Task Ensure(string email,string name,string password,string role) {
   var u=await users.FindByEmailAsync(email);
   if(u==null) { u=new ApplicationUser{UserName=email,Email=email,FullName=name,EmailConfirmed=true,IsActive=true}; var result=await users.CreateAsync(u,password); if(result.Succeeded) await users.AddToRoleAsync(u,role); }
  }
  await Ensure("admin@acxiomcrm.demo","AcxiomCRM Administrator","Admin@123","Admin");
  await Ensure("manager@acxiomcrm.demo","Demo Manager","Manager@123","Manager");
  await Ensure("sales@acxiomcrm.demo","Demo Sales Executive","Sales@123","Sales Executive");
 }
}
