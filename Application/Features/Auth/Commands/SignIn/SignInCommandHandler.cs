using Application.Cqrs.Commands;
using Application.Exceptions.Base;
using Application.Exceptions.ErrorMessages;
using Application.Helpers;
using Application.Identity;
using Application.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auth.Commands.SignIn;

internal class SignInCommandHandler(
    IHttpContextAccessor httpContextAccessor,
    ITokenService tokenService,
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    ITwoFactorTokenSender twoFactorTokenSender,
    IEmailSender emailSender) : ICommandHandler<SignInCommand, SignInDto>
{
    private readonly HttpContext _httpContext = httpContextAccessor.HttpContext!;
    
    public async Task<SignInDto> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var appUser = await userManager.FindByEmailAsync(request.LoginDto.Email);
        if (appUser is null)
        {
            throw new ArgumentValidationException(ErrorMessages.NotFoundUser);
        }

        if (string.IsNullOrEmpty(appUser.PasswordHash))
        {
            throw new ArgumentValidationException(ErrorMessages.CannotAccessToAccountByPassword);
        }
        
        var correctPassword = await userManager.CheckPasswordAsync(appUser, request.LoginDto.Password);
        if (!correctPassword)
        {
            throw new ArgumentValidationException(ErrorMessages.IncorrectPassword);
        }

        if (!appUser.EmailConfirmed)
        {
            var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(appUser);
            var message = EmailMessageHelper.GetEmailConfirmationMessage(confirmationToken, appUser.Id);
            await emailSender.SendEmailAsync(request.LoginDto.Email, message);

            throw new ArgumentValidationException(AuthErrorMessages.EmailNotConfirmed);
        }

        await signInManager.PasswordSignInAsync(appUser, request.LoginDto.Password, false, false);
        
        if (appUser.TwoFactorEnabled)
        {
            var token = await userManager.GenerateTwoFactorTokenAsync(appUser, 
                tokenService.GetTwoFactorTokenProvider(appUser));
            await twoFactorTokenSender.SendAsync(appUser, token);
            
            return new SignInDto
            {
                Code = 202,
                Message = "2FA",
                Tokens = null
            };
        }

        var tokens = await tokenService.GetTokens(appUser, request.LoginDto.RememberMe);
        if (tokens.RefreshToken != null)
        {
            CookieHelper.AddRefreshTokenCookie(_httpContext, tokens.RefreshToken);
        }
        
        return new SignInDto { Tokens = tokens };
    }
}