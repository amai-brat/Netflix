namespace SupportPersistentAPI.Models;

public class FileInfoDto
{
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    public required string Type { get; set; }
    public required Uri Src { get; set; }
    public required string Name { get; set; }
    // public string? Metadata { get; set; }
}