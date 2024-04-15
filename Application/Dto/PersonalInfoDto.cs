namespace Application.Dto;

public class PersonalInfoDto
{
    public string Nickname { get; set; } = null!;
    public string? ProfilePictureUrl { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? BirthDay { get; set; } = null!;
}