using Microsoft.AspNetCore.Identity;

namespace SportsNewsAPI;

public class RoleSeeder
{
    public static async Task SeedRole(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
        var roles = new[] { "Admin", "User", "Moderator"};

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new Role(role));
        }
        
    }
}