namespace Domain;

public class PersonInContent
{
    public int Id { get; set; }
    public int ContentId { get; set; }
    public string Name { get; set; } = null!;
    public Profession Profession { get; set; } = null!;
}