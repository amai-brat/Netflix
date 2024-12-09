using SupportAPI.Models.Abstractions;

namespace SupportAPI.Models;

public class ImageFileInfo: Downloadable
{
    public override FileType Type { get; set; } = FileType.Image;
    public string Name { get; set; } = null!;
}