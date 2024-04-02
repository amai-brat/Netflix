namespace Domain.Dtos;

public class NewSubscriptionDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public int MaxResolution { get; set; } = 720;
    public List<int> AccessibleContentIds { get; set; } = [];
}