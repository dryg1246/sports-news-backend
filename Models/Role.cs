using Microsoft.AspNetCore.Identity;

namespace SportsNewsAPI;

public sealed class Role : IdentityRole<Guid>
{
    public Role(string role) : base()
    {
        Name = role;
        NormalizedName = role.ToUpper();
    }
    
    public Role() : base()
    {
    }
    

}