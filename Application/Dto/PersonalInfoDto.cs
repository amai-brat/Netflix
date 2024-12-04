// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Application.Dto;

[Obsolete("CQRS")]
public class PersonalInfoDto
{
    public string Nickname { get; set; } = null!;
    public string? ProfilePictureUrl { get; set; }
    public string Email { get; set; } = null!;
    public string? BirthDay { get; set; }
}