using System.Text.Json;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using MassTransit;
using Shared.Consts;
using StackExchange.Redis;
using SupportAPI.Services.Abstractions;
using SupportAPI.Services.MetadataExtractor;

namespace SupportAPI.Services.Implementations;

public class FileUploadService(
    IDatabase redisDatabase,
    AmazonS3Client s3Client,
    ILogger<FileUploadService> logger): IFileUploadService
{
    private const string BucketName = "chat-files";
    private const string Policy = """
                                  {
                                      "Version": "2012-10-17",
                                      "Statement": [
                                          {
                                              "Effect": "Allow",
                                              "Principal": { "AWS": ["*"] },
                                              "Action": ["s3:GetObject"],
                                              "Resource": ["arn:aws:s3:::chat-files/*"]
                                          }
                                      ]
                                  }
                                  """;
    public async Task<UploadedFileDto> UploadFileAndSaveMetadataAsync(
        Stream s,
        string contentType,
        Dictionary<string, string> metadata,
        CancellationToken cancellationToken)
    {
        // We'll send this array to permanent s3 storage telling these files are uploaded and ready to be copied
        var guid = NewId.NextGuid();
        var resp = await s3Client.ListBucketsAsync(new ListBucketsRequest() { Prefix = BucketName }, cancellationToken);
        if (resp.Buckets.Count == 0)
        {
            await s3Client.PutBucketAsync(new PutBucketRequest() { BucketName = BucketName }, cancellationToken);
            await s3Client.PutBucketPolicyAsync(new PutBucketPolicyRequest()
            {
                BucketName = BucketName,
                Policy = Policy
            }, cancellationToken);
        }
        var transferUtility = new TransferUtility(s3Client);
        
        try
        {
            var req = new TransferUtilityUploadRequest()
            {
                BucketName = BucketName,
                InputStream = s,
                Key = guid.ToString(),
                ContentType = contentType
            };
            foreach (var mkv in metadata)
            {
                req.Metadata.Add(mkv.Key, mkv.Value);
            }
            await transferUtility.UploadAsync(req, cancellationToken);
            
            // uploaded to temp storage: counter++
            await redisDatabase.HashIncrementAsync(
                RedisKeysConsts.CountersKey, 
                new FieldDto(BucketName, guid.ToString()).ToString());
        }
        catch (Exception ex)
        {
            logger.LogInformation("Error during upload: {Message}", ex.Message);
            throw;
        }
        
        return new UploadedFileDto(
            Name: new FieldDto(BucketName, guid.ToString()),
            Url: s3Client.Config.ServiceURL + BucketName + "/" + guid);
    }
    
    public async Task ExtractFileMetadataToTempDatabaseAsync(FieldDto fieldDto, CancellationToken cancellationToken = default)
    {
        var tempDir = Directory.CreateTempSubdirectory();
        var tempFileName = Path.Combine(tempDir.FullName, fieldDto.ObjectName);
        
        var resp = await s3Client.GetObjectAsync(fieldDto.BucketName, fieldDto.ObjectName, cancellationToken);
        
        var type = resp.Headers.ContentType;
        await resp.WriteResponseStreamToFileAsync(tempFileName, append: false, cancellationToken);

        string serialized = GetSerializedMetadata(tempFileName, type);

        Directory.Delete(tempDir.FullName, recursive: true);
        
        await redisDatabase.HashSetAsync(RedisKeysConsts.MetadataKey, fieldDto.ToString(), serialized);
        
        // uploaded metadata to temp storage: counter++
        await redisDatabase.HashIncrementAsync(RedisKeysConsts.CountersKey, fieldDto.ToString());
    }
    
    public Task DeleteFileAndMetadataAsync(List<string> fileUrls, CancellationToken cancellationToken)
    {
        var deleteFileTasks = new List<Task>();
        foreach (var fileUrl in fileUrls)
        {
            var currentGuid = fileUrl.Split("/").Last();

            var removeArgs = new DeleteObjectRequest
            {
                BucketName = BucketName, 
                Key = currentGuid
            };
            deleteFileTasks.Add(s3Client.DeleteObjectAsync(removeArgs, cancellationToken));
        }

        return Task.WhenAll(deleteFileTasks);
    }

    private string GetSerializedMetadata(string tempFileName, string type)
    {
        try
        {
            var extractor = MetadataExtractorFactory.Create(tempFileName, type);
            var metadata = extractor.ExtractMetadata();
            var serialized = JsonSerializer.Serialize(metadata);
            return serialized;
        }
        catch (Exception e)
        {
            logger.LogInformation("Couldn't extract metadata from {FileName} with {ErrorType}: {Error}", tempFileName, e.GetType().Name, e.Message);
        }

        return JsonSerializer.Serialize(new Dictionary<string, string>());
    }
}