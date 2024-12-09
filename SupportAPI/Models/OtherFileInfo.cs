using SupportAPI.Models.Abstractions;

namespace SupportAPI.Models;

public class OtherFileInfo: Downloadable
{
    public override FileType Type { get; set; } = FileType.Other;
    public string? Name { get; set; }
    public string? Description { get; set; }
}