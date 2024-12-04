namespace Application.Features.Contents.Dtos;

public class SubscriptionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int? MaxResolution { get; set; }
}