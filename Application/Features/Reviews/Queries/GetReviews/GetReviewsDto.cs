namespace Application.Features.Reviews.Queries.GetReviews;

public class GetReviewsDto
{
    public List<ReviewDto> Dtos { get; set; } = null!;
}

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

public class UserDto
{
    public long Id { get; set; }
    public string? Avatar { get; set; }
    public string Name { get; set; } = null!;
}

public class CommentDto
{
    public long Id { get; set; }
    public UserDto User { get; set; } = null!;
    public string Text { get; set; } = null!;
    public DateTimeOffset WrittenAt { get; set; }
}