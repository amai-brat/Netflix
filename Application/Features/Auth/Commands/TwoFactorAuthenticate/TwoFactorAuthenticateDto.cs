using Application.Features.Auth.Dtos;

namespace Application.Features.Auth.Commands.TwoFactorAuthenticate;

public class TwoFactorAuthenticateDto
{
    public TokensDto Tokens { get; set; } = null!;
}