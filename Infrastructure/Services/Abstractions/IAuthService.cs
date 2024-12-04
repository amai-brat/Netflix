using Application.Dto;
using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Commands.SignIn;
using Application.Features.Auth.Commands.TwoFactorAuthenticate;
using Application.Features.Auth.Dtos;

namespace Infrastructure.Services.Abstractions;

[Obsolete("CQRS")]
public interface IAuthService
{
    public Task<long?> RegisterAsync(SignUpDto dto); 
    public Task<TokensDto?> AuthenticateAsync(LoginDto dto);
    public Task<TokensDto> AuthenticateFromExternalAsync(ExternalLoginDto dto);
    public Task<string> ConfirmEmailAsync(long userId, string token);
    public Task<TokensDto> RefreshTokenAsync(string token);
    public Task RevokeTokenAsync(string token);
    public Task<string> ChangePasswordAsync(string userEmail, ChangePasswordDto dto);
    public Task ChangeEmailRequestAsync(string prevEmail, string newEmail);
    public Task<string> ChangeEmailAsync(long userId, string newEmail, string changeToken);
    public Task<string> ChangeRoleAsync(long userId, string newRole);
    public Task EnableTwoFactorAuthAsync(string userEmail);
    public Task<bool> IsEnabledTwoFactorAuthAsync(string userEmail);
    public Task<TokensDto> TwoFactorAuthenticateAsync(TwoFactorTokenDto dto);
}