namespace Application.Dto
{
    [Obsolete("CQRS")]
    public class ReviewAssignDto
    {
        public long ContentId { get; set; }
        public string Text { get; set; } = null!;
        public bool IsPositive { get; set; }
        public int? Score { get; set; }
    }
}
