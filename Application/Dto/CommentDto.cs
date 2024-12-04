namespace Application.Dto;

[Obsolete("CQRS")]
public class CommentDto
{
    public long Id { get; set; }
    public UserDto User { get; set; } = null!;
    public string Text { get; set; } = null!;
    public DateTimeOffset WrittenAt { get; set; }
}