namespace Application.Cache;

public interface IMinioCache
{
    Task RemoveAsync(string key);
    Task<bool> ExistsAsync(string key);
    Task SetStringAsync(string key, string value);
    Task<string?> GetStringAsync(string key);
}