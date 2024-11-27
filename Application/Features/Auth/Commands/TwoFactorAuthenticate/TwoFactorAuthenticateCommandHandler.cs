using Application.Cqrs.Commands;
using Application.Exceptions.Base;
using Application.Exceptions.ErrorMessages;
using Application.Helpers;
using Application.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auth.Commands.TwoFactorAuthenticate;

internal class TwoFactorAuthenticateCommandHandler(
    IHttpContextAccessor httpContextAccessor,
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    ITokenService tokenService) : ICommandHandler<TwoFactorAuthenticateCommand, TwoFactorAuthenticateDto>
{
    private readonly HttpContext _httpContext = httpContextAccessor.HttpContext!;
    
    public async Task<TwoFactorAuthenticateDto> Handle(TwoFactorAuthenticateCommand request, CancellationToken cancellationToken)
    {
        var appUser = await signInManager.GetTwoFactorAuthenticationUserAsync();
        if (appUser == null)
        {
            throw new ArgumentValidationException(ErrorMessages.NotFoundUser);
        }
        
        var result = await userManager.VerifyTwoFactorTokenAsync(
            appUser, 
            tokenService.GetTwoFactorTokenProvider(appUser), 
            request.TwoFactorTokenDto.Token);
        
        if (result)
        {
            var tokens = await tokenService.GetTokens(appUser, request.TwoFactorTokenDto.RememberMe);
            if (tokens.RefreshToken != null)
            {
                CookieHelper.AddRefreshTokenCookie(_httpContext, tokens.RefreshToken);
            }
            
            return new TwoFactorAuthenticateDto { Tokens = tokens };
        }

        throw new ArgumentValidationException(AuthErrorMessages.InvalidTwoFactorToken);
    }
}