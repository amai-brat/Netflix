using Hangfire;
using Shared.Consts;
using StackExchange.Redis;
using SupportPermanentS3Service.Models.Dto;
using SupportPermanentS3Service.Services;

namespace SupportPermanentS3Service.BackgroundServices;

public class TempToPermanentCopierService(
    IRecurringJobManager recurringJobManager,
    IDatabase redisDatabase,
    IFileCopyService fileCopyService,
    IMetadataCopyService metadataCopyService,
    ILogger<TempToPermanentCopierService> logger) : BackgroundService
{
    private const int CanBeDeletedCount = 5;
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        recurringJobManager.AddOrUpdate("copier", 
            () => CheckCountersAsync(stoppingToken), 
            Cron.Minutely);
        
        return Task.CompletedTask;
    }

    public async Task CheckCountersAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Checking counters at {DateTime}", DateTime.UtcNow);
        
        var entries = await redisDatabase.HashGetAllAsync(RedisKeysConsts.CountersKey);
        var fieldsToCopy = GetFieldsToCopy(entries);

        try
        {
            await fileCopyService.CopyFilesAsync(fieldsToCopy, cancellationToken);
            await metadataCopyService.CopyMetadataToDatabaseAsync(fieldsToCopy, cancellationToken);

            // делаю счётчики = CanBeDeletedCount => их можно удалять
            var hashFields = fieldsToCopy
                .Select(x => new HashEntry(x.ToString(), CanBeDeletedCount))
                .ToArray();
            await redisDatabase.HashSetAsync(RedisKeysConsts.CountersKey, hashFields);
        }
        catch (Exception ex)
        {
            logger.LogInformation("Exception occured while copying files to permanent storage: {@Message}", ex);
        }
    }

    private List<FieldDto> GetFieldsToCopy(HashEntry[] entries)
    {
        var result = new List<FieldDto>();
        foreach (var entry in entries)
        {
            if (entry.Value.HasValue && entry.Value.TryParse(out int count))
            {
                logger.LogInformation("{Field} has counter = {Count}", entry.Name, count);

                if (count == 2)
                {
                    result.Add(FieldDto.Parse(entry.Name!));
                }
            }
        }

        return result;
    }
}