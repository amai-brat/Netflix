namespace Application.Dto;

[Obsolete("CQRS")]
public class SubscriptionAdminPageDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int? MaxResolution { get; set; }

}