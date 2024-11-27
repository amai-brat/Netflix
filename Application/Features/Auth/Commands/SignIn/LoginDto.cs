// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Application.Features.Auth.Commands.SignIn;

public class LoginDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public bool RememberMe { get; set; }
}