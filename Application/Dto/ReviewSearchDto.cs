namespace Application.Dto;

public class ReviewSearchDto
{
    public int Page { get; set; }
    public long? UserId { get; set; }
    public string? Search { get; set; }
    public ReviewSortType? SortType { get; set; }
}

public enum ReviewSortType
{
    Rating = 0,
    DateUpdated = 1
}