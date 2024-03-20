namespace Domain;

public class Genre
{
    public int Id { get; set; }

    public string Name { get; set; }  = null!;

    public List<ContentBase> Contents { get; set; }  = null!;
}