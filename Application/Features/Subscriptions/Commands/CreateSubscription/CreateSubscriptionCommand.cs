using Application.Cqrs.Commands;
using Domain.Entities;

namespace Application.Features.Subscriptions.Commands.CreateSubscription;

public record CreateSubscriptionCommand : ICommand<Subscription>
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public int MaxResolution { get; set; } = 720;
    public decimal Price { get; set; }
    public List<long> AccessibleContentIds { get; set; } = [];
}