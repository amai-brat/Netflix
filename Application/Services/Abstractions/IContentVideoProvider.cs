namespace Application.Services.Abstractions;

public interface IContentVideoProvider
{
    Task PutAsync(string name, Stream data, string contentType);
    Task<Stream> GetAsync(string name);
}