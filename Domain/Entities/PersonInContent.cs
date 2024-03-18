namespace Domain;

public class PersonInContent
{
    public int Id { get; set; }
    
    public long ContentId { get; set; }
    public ContentBase Content { get; set; }

    public string Name { get; set; } = null!;

    public int ProfessionId { get; set; }
    public Profession Profession { get; set; } = null!;
}