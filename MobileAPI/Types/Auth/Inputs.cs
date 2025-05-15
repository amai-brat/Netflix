using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Commands.SignIn;

namespace MobileAPI.Types.Auth;

public record SignUpInput(string Login, string Email, string Password)
{
    public SignUpDto ToDto()
    {
        return new SignUpDto
        {
            Login = Login,
            Email = Email,
            Password = Password
        };
    }
}

public record SignInInput(string Email, string Password, bool RememberMe)
{
    public LoginDto ToDto()
    {
        return new LoginDto
        {
            Email = Email,
            Password = Password,
            RememberMe = RememberMe
        };
    }
}

// public record SignOutInput;