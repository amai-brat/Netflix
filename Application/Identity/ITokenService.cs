using Application.Features.Auth.Dtos;

namespace Application.Identity;

public interface ITokenService
{
    Task<TokensDto> GetTokens(AppUser appUser, bool rememberMe);
    string GetTwoFactorTokenProvider(AppUser appUser);
    void RevokeRefreshToken(RefreshToken token, string? reason = null, string? replacedByToken = null);
    Task<TokensDto> RefreshTokenAsync(string token);
}