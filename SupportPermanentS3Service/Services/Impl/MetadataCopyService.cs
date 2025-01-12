using System.Text.Json;
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
            var dick = JsonSerializer.Deserialize<Dictionary<string, string>>(redisValue.ToString())!;
            Console.WriteLine($"{field}:");
            foreach (var (key, value) in dick)
            {
                Console.WriteLine($"\t{key}: {value}");
            }
        }
    }
}