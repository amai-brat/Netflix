using Microsoft.AspNetCore.Identity;

namespace Application.Identity;

public class AppUser : IdentityUser<long>
{
    public TwoFactorType TwoFactorType { get; set; } = TwoFactorType.Email;
}

public enum TwoFactorType
{
    Email = 0,
    Phone = 1,
    App = 2
}