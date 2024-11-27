using Microsoft.AspNetCore.Identity;

// ReSharper disable UnusedMember.Local

namespace Application.Identity;

public sealed class AppRole : IdentityRole<long>
{
    public AppRole(string roleName)
    {
        Name = roleName;
        NormalizedName = roleName.ToUpperInvariant();
    }
    
    private AppRole() {}
}