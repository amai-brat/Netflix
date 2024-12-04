using Application.Helpers;
using Application.Identity;
using Application.Services.Abstractions;

namespace Infrastructure.Services.Implementations;

public class TwoFactorTokenSender(IEmailSender emailSender) : ITwoFactorTokenSender
{
    public async Task SendAsync(AppUser user, string token)
    {
        switch (user.TwoFactorType)
        {
            case TwoFactorType.Email:
                await emailSender.SendEmailAsync(user.Email!, EmailMessageHelper.GetTwoFactorTokenMessage(token));
                break;
            case TwoFactorType.Phone:
                break;
            case TwoFactorType.App:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}