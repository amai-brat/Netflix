using System.Net.Http.Json;
using System.Text;
using Application.Dto;
using Infrastructure.Options;
using Infrastructure.Providers.Abstractions;
using Microsoft.Extensions.Options;

namespace Infrastructure.Providers.Implementations;

public class VkAuthProvider(IOptionsMonitor<VkAuthOptions> monitor): IAuthProvider
{
    private readonly VkAuthOptions _options = monitor.CurrentValue;
    
    public string GetAuthUri()
    {
        var uri = new StringBuilder($"{_options.AuthUri}?");
        
        uri.Append($"client_id={_options.ClientId}");
        uri.Append($"&redirect_uri={_options.RedirectUri}");
        uri.Append("&scope=email");
        uri.Append("&response_type=code");

        return uri.ToString();
    }

    public async Task<ExternalLoginDto> ExchangeCodeAsync(string code)
    {
        try
        {
            using var client = new HttpClient();

            var response = await client.PostAsync(GetExchangeUri(code), new StringContent(""));
            var result = await response.Content.ReadFromJsonAsync<OAuthResponse>();

            if (!result!.IsSuccess)
                return new ExternalLoginDto { Error = result.Error, ErrorDescription = result.ErrorDescription };
            
            var personInfoResponse = await client.PostAsync(GetInfoUri(result.AccessToken), new StringContent(""));
            var personInfoResult = (await personInfoResponse.Content.ReadFromJsonAsync<AuthVkResponse>())!.AuthVkPersonInfos.First();
            
            return new ExternalLoginDto
            {
                Login = personInfoResult.FullName,
                Email = result.Email,
                PictureUrl = personInfoResult.Photo
            };
        }
        catch (Exception)
        {
            return null!;
        }
    }

    private string GetInfoUri(string accessToken)
    {
        var uri = new StringBuilder($"{_options.InfoReqUri}?");
        
        uri.Append("fields=photo_50");
        uri.Append($"&access_token={accessToken}");
        uri.Append("&v=5.199");

        return uri.ToString();
    }

    private string GetExchangeUri(string code)
    {
        var uri = new StringBuilder($"{_options.TokenUri}?");
        
        uri.Append($"code={code}");
        uri.Append($"&client_id={_options.ClientId}");
        uri.Append($"&client_secret={_options.ClientSecret}");
        uri.Append($"&redirect_uri={_options.RedirectUri}");
        uri.Append("&grant_type=authorization_code");
        uri.Append("&scope=email");

        return uri.ToString();
    }
}