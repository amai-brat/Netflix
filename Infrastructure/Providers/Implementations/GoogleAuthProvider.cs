using System.Net.Http.Json;
using System.Text;
using Application.Dto;
using Infrastructure.Options;
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

    public async Task<OAuthResponse> ExchangeCodeAsync(string code)
    {
        var dicData = new Dictionary<string, string>
        {
            ["code"] = code,
            ["client_id"] = _options.ClientId,
            ["client_secret"] = _options.ClientSecret,
            ["redirect_uri"] = _options.RedirectUri,
            ["grant_type"] = "authorization_code",
            ["scope"] = "https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile"
        };
            
        try
        {
            using var client = new HttpClient();
            using var content = new FormUrlEncodedContent(dicData);
            var response = await client.PostAsync(_options.TokenUri, content);
            var result = await response.Content.ReadFromJsonAsync<OAuthResponse>();
            return result!;
        }
        catch (Exception)
        {
            return null!;
        }
    }
}