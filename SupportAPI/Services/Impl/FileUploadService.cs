using System.Text.Json;
using MassTransit;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using StackExchange.Redis;
using SupportAPI.Models.Dto;
using SupportAPI.Options;

namespace SupportAPI.Services.Impl;

public class FileUploadService(
    IMinioClient minio,
    IOptions<MinioBucketOptions> minioOptions,
    IDatabase database): IFileUploadService
{
    public async Task<List<Guid>> UploadFileAsync(UploadMessageWithFIleDto uploadMessageWithFIleDto)
    {
        // We'll send this array to permanent s3 storage telling these files are ready to be uploaded
        var fileGuids = new List<Guid>();

        // Upload files metadata + counter to Redis
        foreach (var file in uploadMessageWithFIleDto.Files)
        {
            var fileGuid = NewId.NextGuid();
            fileGuids.Add(fileGuid);
            
            var transaction = database.CreateTransaction();
            await transaction.StringSetAsync(
                new RedisKey(fileGuid.ToString()).Prepend("metadata"),
                new RedisValue(JsonSerializer.Serialize(uploadMessageWithFIleDto)),
                TimeSpan.FromDays(1));
            await transaction.StringIncrementAsync(new RedisKey(fileGuid.ToString()).Prepend("counter"));
            await transaction.ExecuteAsync();
        }
        
        // Upload files to Minio
        var bucketName = "chat-files";
        var bucketExistsArgs = new BucketExistsArgs().WithBucket(bucketName);
        bool found = await minio.BucketExistsAsync(bucketExistsArgs);
        if (!found)
        {
            var makeBucketArgs = new MakeBucketArgs().WithBucket(bucketName);
            var setPolicyArgs = new SetPolicyArgs().WithBucket(bucketName)
                .WithPolicy(minioOptions.Value.FileBucketPolicy);
            await minio.MakeBucketAsync(makeBucketArgs);
            await minio.SetPolicyAsync(setPolicyArgs);
        }

        var minioUploadTasks = new List<Task>();
        var redisUploadTasks = new List<Task>();
        for (int i = 0; i < uploadMessageWithFIleDto.Files.Count; i++)
        {
            var putArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithFileName(fileGuids[i].ToString())
                .WithStreamData(uploadMessageWithFIleDto.Files[i].File.OpenReadStream())
                .WithObjectSize(uploadMessageWithFIleDto.Files[i].File.Length);
                
                
            minioUploadTasks.Add(minio.PutObjectAsync(putArgs));
            redisUploadTasks.Add(database.StringIncrementAsync(fileGuids[i].ToString()));
        }

        await Task.WhenAll(minioUploadTasks);
        await Task.WhenAll(redisUploadTasks);
        
        return fileGuids;
    }
}