using SupportAPI.Models.Abstractions;

namespace SupportAPI.Models.Dto;

public class UploadMessageWithFIleDto
{
    public string? Message { get; set; }
    public required List<IHavingFormFile> Files { get; set; }
}