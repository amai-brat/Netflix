// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Application.Dto;

[Obsolete("CQRS")]
public class AdminSubscriptionContentDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
}