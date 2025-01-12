using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using Shared.Consts;
using StackExchange.Redis;
using SupportPermanentS3Service.Enums;
using SupportPermanentS3Service.Models.Dto;

namespace SupportPermanentS3Service.Services.Impl;

public class TempCleanerService(
    [FromKeyedServices(KeyedMinios.Temporary)] IMinioClient minioClient,
    IDatabase redisDatabase,
    ILogger<TempCleanerService> logger) : ITempCleanerService
{
    private const int CanBeDeletedCount = 5;

    public async Task CleanTempAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Cleaning temp at {DateTime}", DateTime.UtcNow);
    
        var entries = await redisDatabase.HashGetAllAsync(RedisKeysConsts.CountersKey);
        var fieldsToClean = await GetFieldsToCleanAsync(entries);
        
        await CleanTempStorageAsync(fieldsToClean, cancellationToken);
        await CleanTempMetadataAsync(fieldsToClean, cancellationToken);
        
        await CleanCountersAsync(fieldsToClean, cancellationToken);
    }
    
    private async Task<List<FieldDto>> GetFieldsToCleanAsync(HashEntry[] entries)
    {
        List<FieldDto> fieldsToClean = [];
        foreach (var entry in entries)
        {
            if (!entry.Value.HasValue || !entry.Value.TryParse(out int count)) continue;
            
            if (count == CanBeDeletedCount)
            {
                fieldsToClean.Add(FieldDto.Parse(entry.Name!));
                await redisDatabase.HashDeleteAsync(RedisKeysConsts.SuspiciosCountersKey, entry.Name);
            }
            else
            {
                await redisDatabase.HashIncrementAsync(RedisKeysConsts.SuspiciosCountersKey, entry.Name);
                    
                var value = await redisDatabase.HashGetAsync(RedisKeysConsts.SuspiciosCountersKey, entry.Name);
                if (value.TryParse(out int susCount) && susCount > 1)
                {
                    logger.LogWarning("{Field} wasn't handled {Count} times", entry.Name, susCount);
                }
            }
        }
    
        return fieldsToClean;
    }
    
    private async Task CleanTempStorageAsync(List<FieldDto> fields, CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (var (bucket, @object) in fields)
            {
                await minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                    .WithBucket(bucket)
                    .WithObject(@object), cancellationToken);
            }
            
        }
        catch (ObjectNotFoundException e)
        {
            logger.LogWarning("Temp minio: {@Exception}", e);
        }
    }
    
    private async Task CleanTempMetadataAsync(List<FieldDto> fields, CancellationToken _ = default)
    {
        var array = fields
            .Select(x => new RedisValue(x.ToString()))
            .ToArray();
        
        await redisDatabase.HashDeleteAsync(RedisKeysConsts.MetadataKey, array);
    }
    
    private async Task CleanCountersAsync(List<FieldDto> fields, CancellationToken _ = default)
    {
        var array = fields
            .Select(x => new RedisValue(x.ToString()))
            .ToArray();
        
        await redisDatabase.HashDeleteAsync(RedisKeysConsts.CountersKey, array);
    }
}