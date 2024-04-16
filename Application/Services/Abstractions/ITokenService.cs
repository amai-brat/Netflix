using Application.Dto;
using Domain.Entities;

namespace Application.Services.Abstractions;

public interface ITokenService
{
    public Task<TokensDto> GenerateTokensAsync(User user, bool withRefreshToken = true);
    public Task<TokensDto> RefreshTokenAsync(string token);
    public Task RevokeTokenAsync(string token);
}