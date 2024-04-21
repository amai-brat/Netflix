namespace Application.Dto;

public class UserDto
{
    public long Id { get; set; }
    public string? Avatar { get; set; }
    public string Name { get; set; } = null!;
}