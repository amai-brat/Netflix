namespace Application.Features.Users.Queries.GetPersonalInfo;

public class PersonalInfoDto
{
    public string Nickname { get; set; } = null!;
    public string? ProfilePictureUrl { get; set; }
    public string Email { get; set; } = null!;
    public string? BirthDay { get; set; }
}