using Application.Features.Auth.Dtos;

namespace Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenDto
{
    public int Code { get; set; }
    public string? Message { get; set; }
    public TokensDto? Tokens { get; set; }
}