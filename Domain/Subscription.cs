namespace Domain;

public class Subscription
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime BoughtAt { get; set; }
    public List<MovieContent> accessibleMovies { get; set; }
    public List<SerialContent> accessibleSerials { get; set; }
}