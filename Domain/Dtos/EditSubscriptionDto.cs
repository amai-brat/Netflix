namespace Domain.Dtos;

public class EditSubscriptionDto
{
    public int SubscriptionId { get; set; }
    public string? NewName { get; set; }
    public string? NewDescription { get; set; }
    public int? NewMaxResolution { get; set; }
    public List<long>? AccessibleContentIdsToAdd { get; set; }
    public List<long>? AccessibleContentIdsToRemove { get; set; }
}