using Application.Dto;

namespace Infrastructure.Services.Abstractions;

public interface IAuthService
{
    public Task<long?> RegisterAsync(SignUpDto dto); 
    public Task<TokensDto> AuthenticateAsync(LoginDto dto);
    public Task<string> ConfirmEmailAsync(long userId, string token);
    public Task<TokensDto> RefreshTokenAsync(string token);
    public Task RevokeTokenAsync(string token);
    public Task<string> ChangePasswordAsync(string userEmail, ChangePasswordDto dto);
    public Task ChangeEmailRequestAsync(string prevEmail, string newEmail);
    public Task<string> ChangeEmailAsync(long userId, string newEmail, string changeToken);
    public Task<string> ChangeRoleAsync(long userId, string newRole);
}