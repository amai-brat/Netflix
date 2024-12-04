namespace Application.Dto;

[Obsolete("CQRS")]
public class NewSubscriptionDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public int MaxResolution { get; set; } = 720;
    public decimal Price { get; set; }
    public List<long> AccessibleContentIds { get; set; } = [];
}