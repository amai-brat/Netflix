namespace Domain;

public class Subscription
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public DateTime BoughtAt { get; set; }
    public List<ContentBase> AccessibleContent { get; set; } = null!;
}