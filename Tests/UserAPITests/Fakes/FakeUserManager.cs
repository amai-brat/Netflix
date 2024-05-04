using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Tests.UserAPITests.Fakes;

public class FakeUserManager(List<User> users, List<AppUser> appUsers) : UserManager<AppUser>(
    Mock.Of<IUserStore<AppUser>>(),
    Mock.Of<IOptions<IdentityOptions>>(),
    Mock.Of<IPasswordHasher<AppUser>>(),
    Array.Empty<IUserValidator<AppUser>>(),
    Array.Empty<IPasswordValidator<AppUser>>(),
    Mock.Of<ILookupNormalizer>(),
    Mock.Of<IdentityErrorDescriber>(),
    Mock.Of<IServiceProvider>(),
    Mock.Of<ILogger<UserManager<AppUser>>>())
{
    public override Task<IdentityResult> CreateAsync(AppUser user, string password)
    {
        return Task.FromResult(IdentityResult.Success);
    }

    public override Task<IdentityResult> AddToRoleAsync(AppUser user, string role)
    {
        return Task.FromResult(IdentityResult.Success);
    }

    public override Task<AppUser?> FindByEmailAsync(string email)
    {
        return Task.FromResult(appUsers.FirstOrDefault(x => x.Email == email));
    }

    public override Task<AppUser?> FindByIdAsync(string userId)
    {
        return Task.FromResult(appUsers.FirstOrDefault(x => x.Id == long.Parse(userId)));
    }

    public override Task<IdentityResult> ChangeEmailAsync(AppUser user, string newEmail, string token)
    {
        return Task.FromResult(IdentityResult.Success);
    }

    public override Task<string> GenerateEmailConfirmationTokenAsync(AppUser user)
    {
        return Task.FromResult(Guid.NewGuid().ToString());
    }

    public override Task<bool> CheckPasswordAsync(AppUser user, string password)
    {
        return Task.FromResult(password == user.PasswordHash);
    }

    public override Task<IList<string>> GetRolesAsync(AppUser user)
    {
        return Task.FromResult((IList<string>)new List<string>{Guid.NewGuid().ToString()});
    }
}
