namespace MobileAPI.Models;

public class ChatMessageDto
{
    public string Role { get; set; } = null!;
    public List<FileInfoDto>? Files { get; set; }
    public string Text { get; set; } = null!;
}

public class FileInfoDto
{
    public required string Type { get; set; }
    public required Uri Src { get; set; }
    public required string Name { get; set; }
}