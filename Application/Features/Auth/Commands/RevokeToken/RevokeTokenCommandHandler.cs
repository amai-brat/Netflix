using Application.Cqrs.Commands;
using Application.Exceptions.ErrorMessages;
using Application.Identity;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Auth.Commands.RevokeToken;

internal class RevokeTokenCommandHandler(
    ITokenService tokenService,
    IIdentityUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor,
    ITokenRepository tokenRepository) : ICommandHandler<RevokeTokenCommand, RevokeTokenDto>
{
    private readonly HttpContext _httpContext = httpContextAccessor.HttpContext!;
    
    public async Task<RevokeTokenDto> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshTokenStr = _httpContext.Request.Cookies["refresh-token"];
        if (refreshTokenStr is null)
        {
            return new RevokeTokenDto
            {
                Code = 401,
                Error = "Refresh token is not found"
            };
        }
        var refreshToken = await tokenRepository.GetRefreshTokenWithUserByTokenAsync(refreshTokenStr);

        if (refreshToken is null)
            throw new Exception(ErrorMessages.RefreshTokenNotFound);

        if (!refreshToken.IsActive)
            throw new Exception(ErrorMessages.NotActiveRefreshToken);
        
        tokenService.RevokeRefreshToken(refreshToken, "Revoked without replacement");

        _httpContext.Response.Cookies.Delete("refresh-token");
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new RevokeTokenDto
        {
            Code = 204
        };
    }
}