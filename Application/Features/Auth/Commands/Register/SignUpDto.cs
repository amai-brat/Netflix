namespace Application.Features.Auth.Commands.Register;

public class SignUpDto
{
    public string Login { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}