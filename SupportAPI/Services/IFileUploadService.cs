using SupportAPI.Models.Dto;

namespace SupportAPI.Services;

public interface IFileUploadService
{
    public Task<List<Guid>> UploadFileAsync(UploadMessageWithFIleDto uploadMessageWithFIleDto);
}