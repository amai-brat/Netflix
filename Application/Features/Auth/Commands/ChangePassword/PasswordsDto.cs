namespace Application.Features.Auth.Commands.ChangePassword;

public class PasswordsDto
{
    public string PreviousPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}