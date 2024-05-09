using Application.Dto;

namespace Infrastructure.Providers.Abstractions;

public interface IAuthProvider
{
    string GetAuthUri();
    Task<OAuthResponse> ExchangeCodeAsync(string code);
}