using Application.Cache;
using Application.Exceptions.Base;
using Application.Exceptions.ErrorMessages;
using StackExchange.Redis;

namespace DataAccess.Cache;

public class MinioRedisCache: IMinioCache
{
    private IDatabase Database { get; }
    
    public MinioRedisCache(IDatabase database)
    {
        Database = database;
    }

    public async Task RemoveAsync(string key)
    {
        await Database.KeyDeleteAsync(key);
    }

    public async Task<bool> ExistsAsync(string key)
    {
        return await Database.KeyExistsAsync(key);
    }

    public async Task SetStringAsync(string key, string value)
    {
        var current = await Database.StringGetAsync(key);
        if (current.HasValue)
        {
            var transaction = Database.CreateTransaction();
            transaction.AddCondition(Condition.StringEqual(key, current));
            await transaction.StringSetAsync(key, value);
            var res = await transaction.ExecuteAsync();
            if (!res)
            {
                throw new ValueChangedException(UserCacheErrorMessages.AvatarChangedDuringRequest);
            }
        }
        else
        {
            Database.StringSet(key, value);
        }
    }

    public async Task<string?> GetStringAsync(string key)
    {
        return await Database.StringGetAsync(key);
    }
}