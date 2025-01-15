using System.Text.Json;
using Shared.Consts;
using StackExchange.Redis;
using SupportPermanentS3Service.Data.Entities;
using SupportPermanentS3Service.Models.Dto;
using SupportPermanentS3Service.Repositories;
using File = SupportPermanentS3Service.Data.Entities.File;

namespace SupportPermanentS3Service.Services.Impl;

public class MetadataCopyService(
    IDatabase redisDatabase,
    IFileRepository fileRepository,
    IMetadataRepository metadataRepository,
    IMetadataValueRepository metadataValueRepository,
    IUnitOfWork unitOfWork) : IMetadataCopyService
{
    public async Task CopyMetadataToDatabaseAsync(List<FieldDto> fieldsToCopy, CancellationToken cancellationToken = default)
    {
        foreach (var field in fieldsToCopy)
        {
            var redisValue = await redisDatabase.HashGetAsync(RedisKeysConsts.MetadataKey, field.ToString());
            var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(redisValue.ToString())!;

            var file = new File
            {
                Name = field.ObjectName
            };
            file = await fileRepository.AddFileAsync(file);
            
            foreach (var (metadataName, value) in dict)
            {
                var metadata = await metadataRepository.GetByNameAsync(metadataName) 
                                 ?? await metadataRepository.AddMetadataAsync(new Metadata { Name = metadataName });

                var metadataValue = new MetadataValue
                {
                    File = file,
                    Metadata = metadata,
                    Value = value
                };

                await metadataValueRepository.AddMetadataValueAsync(metadataValue);
            }
        }

        await unitOfWork.SaveChangesAsync();
    }
}