using Shared;

namespace Infrastucture.Services;

public interface IProfilePicturesProvider
{
    public Task<Result> PutAsync(string name, Stream pictureStream, string contentType);
    public Task<Result<Stream>> GetAsync(string name);
    public Task<Result<string>> GetUrlAsync(string name);
}