using Microsoft.AspNetCore.Identity;

namespace SportsNewsAPI.Role;

public class RoleSeeder
{
    public static async Task SeedRole(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var roles = new[] { "Admin", "User" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
        }
        
    }
}