using Application.Dto;

namespace Infrastructure.Providers.Implementations;

public class VkAuthProvider: IAuthProvider
{
    public string GetAuthUri()
    {
        throw new NotImplementedException();
    }

    public Task<OAuthResponse> ExchangeCodeAsync(string code)
    {
        throw new NotImplementedException();
    }
}