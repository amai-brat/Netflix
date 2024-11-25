namespace Application.Features.Users.Queries.GetReviews;

public class GetReviewsDto
{
    public List<UserReviewDto> ReviewDtos { get; set; } = null!;
}

public class UserReviewDto
{
    public long Id { get; set; }
    public bool IsPositive { get; set; }
    public string Name { get; set; } = null!;
    public int Score { get; set; }
    public string Text { get; set; } = null!;
    public string ContentName { get; set; } = null!;
    public DateTimeOffset WrittenAt { get; set; }
}