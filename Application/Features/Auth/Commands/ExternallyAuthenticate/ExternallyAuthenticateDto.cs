using Application.Features.Auth.Dtos;

namespace Application.Features.Auth.Commands.ExternallyAuthenticate;

public class ExternallyAuthenticateDto
{
    public int Code { get; set; }
    public string? Message { get; set; }
    public TokensDto? Tokens { get; set; }
}