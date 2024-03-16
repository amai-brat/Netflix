namespace Domain;

public class Subscription
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTimeOffset ExpiresAt { get; set; }
    public DateTimeOffset BoughtAt { get; set; }
    public List<ContentBase> AccessibleContent { get; set; } = null!;
}