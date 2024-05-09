using Infrastructure.Identity;

namespace Infrastructure.Services.Abstractions;

public interface ITwoFactorTokenSender
{
    public Task SendAsync(AppUser user, string token);
}