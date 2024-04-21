namespace Application.Dto;

public class ReviewDto
{
    public long Id { get; set; }
    public UserDto User { get; set; } = null!;
    public int Score { get; set; }
    public DateTimeOffset WrittenAt { get; set; }
    public int LikesScore { get; set; }
    public bool IsPositive { get; set; }
    public string Text { get; set; } = null!;
    public List<CommentDto> Comments { get; set; } = [];
}