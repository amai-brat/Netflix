namespace Domain.Entities;

public class Subscription
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int MaxResolution { get; set; }

    // В условных единицах. Для цен в определённой валюте понадобится ICurrencyConverter
    public decimal Price { get; set; }

    public List<ContentBase> AccessibleContent { get; set; } = null!;
}