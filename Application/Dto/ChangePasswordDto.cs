namespace Application.Dto;

[Obsolete("CQRS")]
public class ChangePasswordDto
{
    public string PreviousPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}