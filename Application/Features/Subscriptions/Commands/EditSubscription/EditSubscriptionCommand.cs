using Application.Cqrs.Commands;
using Domain.Entities;

namespace Application.Features.Subscriptions.Commands.EditSubscription;

public class EditSubscriptionCommand : ICommand<Subscription>
{
    public int SubscriptionId { get; set; }
    public string? NewName { get; set; }
    public string? NewDescription { get; set; }
    public int? NewMaxResolution { get; set; }
    public decimal? NewPrice { get; set; }
    public List<long>? AccessibleContentIdsToAdd { get; set; }
    public List<long>? AccessibleContentIdsToRemove { get; set; }
}