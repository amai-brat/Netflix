using Application.Dto;

namespace Infrastructure.Providers;

public interface IAuthProvider
{
    string GetAuthUri();
    Task<OAuthResponse> ExchangeCodeAsync(string code);
}