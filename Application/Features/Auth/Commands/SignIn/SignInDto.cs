using Application.Features.Auth.Dtos;

namespace Application.Features.Auth.Commands.SignIn;

public class SignInDto
{
    public int Code { get; set; }
    public string? Message { get; set; }
    public TokensDto? Tokens { get; set; }
}