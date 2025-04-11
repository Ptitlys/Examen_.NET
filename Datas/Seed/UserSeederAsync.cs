using Examen.Models;
using Examen.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace Examen.Datas.Seed;

public class UserSeederAsync : ISeederAsync
{
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;

    public UserSeederAsync(RoleManager<Role> roleManager, UserManager<User> userManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }   
    
    public async Task SeedAsync(ApplicationDbContext context)
    {
        var roles = new string[] {"Admin", "Technician", "Customer"};
        if (!context.Roles.Any())
        {
            foreach (var roleName in roles)
            {
                await _roleManager.CreateAsync(new Role { Name = roleName });
            }
        }
        
        if (!context.Users.Any())
        {
            // Creation user admin
            var user = new User { UserName = "admin@example.com", Email = "admin@example.com" };
            await _userManager.CreateAsync(user, "Admin@123"); 
            await _userManager.AddToRoleAsync(user, "Admin");  
        }
        
        
    }
}