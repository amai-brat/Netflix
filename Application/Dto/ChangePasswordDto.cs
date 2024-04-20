namespace Application.Dto;

public class ChangePasswordDto
{
    public string PreviousPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}