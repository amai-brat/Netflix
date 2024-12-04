namespace Application.Features.Auth.Commands.RevokeToken;

public class RevokeTokenDto
{
    public int Code { get; set; }
    public string? Error { get; set; }
}