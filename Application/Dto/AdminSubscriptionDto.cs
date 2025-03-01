// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Application.Dto;

[Obsolete("CQRS")]
public class AdminSubscriptionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int MaxResolution { get; set; }
    public decimal Price { get; set; }

    public List<AdminSubscriptionContentDto> AccessibleContent { get; set; } = null!;
    
}