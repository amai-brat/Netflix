using Application.Cqrs.Commands;
using Application.Helpers;
using Application.Identity;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Auth.Commands.RefreshToken;

internal class RefreshTokenCommandHandler(
    IHttpContextAccessor httpContextAccessor,
    ITokenService tokenService) : ICommandHandler<RefreshTokenCommand, RefreshTokenDto>
{
    private readonly HttpContext _httpContext = httpContextAccessor.HttpContext!;
    
    public async Task<RefreshTokenDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = _httpContext.Request.Cookies["refresh-token"];
        if (refreshToken is null)
        {
            return new RefreshTokenDto
            {
                Code = 401,
                Message = "Refresh token is not found"
            };
        }

        var tokens = await tokenService.RefreshTokenAsync(refreshToken);
        if (tokens.RefreshToken != null)
        {
            CookieHelper.AddRefreshTokenCookie(_httpContext, tokens.RefreshToken);
        }

        return new RefreshTokenDto
        {
            Code = 200,
            Tokens = tokens
        };
    }
}