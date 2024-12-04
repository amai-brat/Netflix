using Application.Features.Auth.Dtos;

namespace Application.Providers;

public interface IAuthProvider
{
    string GetAuthUri();
    Task<ExternalLoginDto> ExchangeCodeAsync(string code);
}