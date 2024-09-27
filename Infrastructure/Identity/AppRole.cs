using Microsoft.AspNetCore.Identity;
// ReSharper disable UnusedMember.Local

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