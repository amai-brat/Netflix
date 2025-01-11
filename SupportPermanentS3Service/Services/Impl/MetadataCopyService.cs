using Shared.Consts;
using StackExchange.Redis;
using SupportPermanentS3Service.Models.Dto;

namespace SupportPermanentS3Service.Services.Impl;

public class MetadataCopyService(
    IDatabase redisDatabase) : IMetadataCopyService
{
    // TODO: поднять EAV postgresql
    public async Task CopyMetadataToDatabaseAsync(List<FieldDto> fieldsToCopy, CancellationToken cancellationToken = default)
    {
        foreach (var field in fieldsToCopy)
        {
            var redisValue = await redisDatabase.HashGetAsync(RedisKeysConsts.MetadataKey, field.ToString());
            Console.WriteLine($"{field}: {redisValue.ToString()}");
        }
    }
}