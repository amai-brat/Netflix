namespace Application.Identity;

public interface ITwoFactorTokenSender
{
    public Task SendAsync(AppUser user, string token);
}