namespace Domain;

public class ContentType
{
    public int Id { get; set; }
    public string ContentTypeName { get; set; } = null!;

    public List<ContentBase>? ContentsWithType { get; set; }
}