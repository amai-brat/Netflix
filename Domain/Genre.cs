namespace ClassLibrary1;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<MovieContent> Movies { get; set; }
}