using System.Text.Json.Serialization;

namespace Application.Dto;

public class AuthVkResponse
{
    [JsonPropertyName("response")] 
    public List<AuthVkPersonInfo> AuthVkPersonInfos { get; set; } = null!;
}