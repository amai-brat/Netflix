namespace Domain;

public class Genre
{
    public string Name { get; set; }  = null!;

    public List<ContentBase> Contents { get; set; }  = null!;
}