namespace Application.Providers;

public interface IProfilePicturesProvider
{
    public Task PutAsync(string name, Stream pictureStream, string contentType);
    public Task<Stream> GetAsync(string name);
    public Task<string> GetUrlAsync(string name);
}