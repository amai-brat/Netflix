using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Text;
using Application.Dto;
using Infrastructure.Options;
using Infrastructure.Providers.Abstractions;
using Microsoft.Extensions.Options;

namespace Infrastructure.Providers.Implementations;

public class GoogleAuthProvider(IOptionsMonitor<GoogleAuthOptions> monitor): IAuthProvider
{
    private readonly GoogleAuthOptions _options = monitor.CurrentValue;
    
    public string GetAuthUri()
    {
        var uri = new StringBuilder($"{_options.AuthUri}?");
        
        uri.Append($"client_id={_options.ClientId}");
        uri.Append($"&redirect_uri={_options.RedirectUri}");
        uri.Append("&response_type=code");
        uri.Append("&scope=https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile");

        return uri.ToString();
    }

    public async Task<ExternalLoginDto> ExchangeCodeAsync(string code)
    {
        try
        {
            using var client = new HttpClient();
            using var content = new FormUrlEncodedContent(GetDictParams(code));
            var response = await client.PostAsync(_options.TokenUri, content);
            var result = await response.Content.ReadFromJsonAsync<OAuthResponse>();
            
            if (!result!.IsSuccess)
                return new ExternalLoginDto { Error = result.Error, ErrorDescription = result.ErrorDescription };
            
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(result!.IdToken);
            
            return new ExternalLoginDto
            {
                Login = jwt.Claims.First(c => c.Type == "name").Value,
                Email = jwt.Claims.First(c => c.Type == "email").Value,
                PictureUrl = jwt.Claims.First(c => c.Type == "picture").Value
            };
        }
        catch (Exception)
        {
            return null!;
        }
    }

    private Dictionary<string, string> GetDictParams(string code) =>
        new()
        {
            ["code"] = code,
            ["client_id"] = _options.ClientId,
            ["client_secret"] = _options.ClientSecret,
            ["redirect_uri"] = _options.RedirectUri,
            ["grant_type"] = "authorization_code",
            ["scope"] = "https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile"
        };
}