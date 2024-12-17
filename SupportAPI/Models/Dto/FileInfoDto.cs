
namespace SupportAPI.Models.Dto;

public class FileInfoDto
{
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public required string Type { get; set; }
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public required Uri Src { get; set; }
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public required string Name { get; set; }
    // public string? Metadata { get; set; }
}