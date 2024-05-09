using System.Text.Json.Serialization;

namespace Application.Dto;

public class OAuthResponse
{

    [JsonPropertyName("access_token")] 
    public string AccessToken { get; set; } = null!;
    [JsonPropertyName("expires_in")] 
    public int ExpiresIn { get; set; }
    [JsonPropertyName("refresh_token")] 
    public string RefreshToken { get; set; } = null!;
    [JsonPropertyName("scope")] 
    public string Scope { get; set; } = null!;
    [JsonPropertyName("token_type")] 
    public string TokenType { get; set; } = null!;
    [JsonPropertyName("id_token")] 
    public string IdToken { get; set; } = null!;
    [JsonPropertyName("email")] 
    public string Email { get; set; } = null!;
    
    [JsonPropertyName("error")] 
    public string Error { get; set; } = null!;
    [JsonPropertyName("error_description")]
    public string ErrorDescription { get; set; } = null!;

    public bool IsSuccess => string.IsNullOrEmpty(Error);
}