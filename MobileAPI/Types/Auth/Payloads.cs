using Application.Features.Auth.Commands.RevokeToken;
using Application.Features.Auth.Commands.SignIn;
using Application.Features.Auth.Dtos;

namespace MobileAPI.Types.Auth;

public record SignUpPayload(long? Id);

public record SignInPayload(int Code, string? Message, TokensDto? Tokens)
{
    public static SignInPayload From(SignInDto dto)
    {
        return new SignInPayload(dto.Code, dto.Message, dto.Tokens);
    }
}

public record SignOutPayload(int Code, string? Message)
{
    public static SignOutPayload From(RevokeTokenDto dto)
    {
        return new SignOutPayload(dto.Code, dto.Error);
    }
}
