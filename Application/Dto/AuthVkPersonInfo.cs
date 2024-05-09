using System.Text.Json.Serialization;

namespace Application.Dto;

public class AuthVkPersonInfo
{
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; } = null!;
    [JsonPropertyName("last_name")]
    public string LastName { get; set; } = null!;
    [JsonPropertyName("photo_50")] 
    public string Photo { get; set; } = null!;
    public string FullName => $"{LastName} {FirstName}";
}