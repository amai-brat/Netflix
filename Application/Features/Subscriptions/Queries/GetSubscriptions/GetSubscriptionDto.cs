using Application.Features.Subscriptions.Queries.GetAvailableContents;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Application.Features.Subscriptions.Queries.GetSubscriptions;

public class GetSubscriptionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int MaxResolution { get; set; }
    public decimal Price { get; set; }
    public List<SubscriptionContentDto> AccessibleContent { get; set; } = null!;
}