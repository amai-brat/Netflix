using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Tests.UserAPITests.Fakes;

public class FakeSignInManager(FakeUserManager fakeUserManager) : SignInManager<AppUser>(
    fakeUserManager,
    Mock.Of<IHttpContextAccessor>(),
    Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(),
    Mock.Of<IOptions<IdentityOptions>>(),
    Mock.Of<ILogger<SignInManager<AppUser>>>(),
    Mock.Of<IAuthenticationSchemeProvider>(),
    Mock.Of<IUserConfirmation<AppUser>>())
{
    public override Task<SignInResult> PasswordSignInAsync(AppUser user, string password, bool isPersistent, bool lockoutOnFailure)
    {
        return Task.FromResult(SignInResult.Success);
    }
}