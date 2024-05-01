using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public sealed class AppRole : IdentityRole<long>
{
    public AppRole(string roleName)
    {
        Name = roleName;
        NormalizedName = roleName.ToUpperInvariant();
    }
    
    private AppRole() {}
}