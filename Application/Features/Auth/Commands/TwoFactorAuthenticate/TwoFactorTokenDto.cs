// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Application.Features.Auth.Commands.TwoFactorAuthenticate;

public class TwoFactorTokenDto
{
    public string Token { get; set; } = null!;
    public bool RememberMe { get; set; }
}