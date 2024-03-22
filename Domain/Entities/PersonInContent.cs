namespace Domain.Entities;

public class PersonInContent
{
    public int Id { get; set; }
    
    public long ContentId { get; set; }
    public ContentBase Content { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int ProfessionId { get; set; }
    public Profession Profession { get; set; } = null!;
}