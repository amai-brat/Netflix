using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using MassTransit;
using Minio;
using Minio.DataModel.Args;

namespace SupportAPI.Services.Impl;

public class FileUploadService(
    IMinioClient minio,
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
    public async Task<string> UploadFileAndSaveMetadataAsync(Stream s)
    {
        // We'll send this array to permanent s3 storage telling these files are uploaded and ready to be copied
        Guid guid = NewId.NextGuid();
        var resp = await s3Client.ListBucketsAsync(new ListBucketsRequest(){Prefix = BucketName});
        if (resp.Buckets.Count == 0)
        {
            await s3Client.PutBucketAsync(new PutBucketRequest(){BucketName = BucketName});
            await s3Client.PutBucketPolicyAsync(new PutBucketPolicyRequest()
            {
                BucketName = BucketName,
                Policy = Policy
            });
        }
        var transferUtility = new TransferUtility(s3Client);
        
        try
        {
            await transferUtility.UploadAsync(s, BucketName, guid.ToString());
        }
        catch (Exception ex)
        {
            logger.LogInformation($"Error during upload: {ex.Message}");
        }
        return s3Client.Config.ServiceURL + BucketName + "/" + guid;
    }

    public string GetBucketName()
    {
        return BucketName;
    }
    
    public Task DeleteFileAndMetadataAsync(List<string> fileUrls)
    {
        // var deleteMetadataTasks = new List<Task>();
        var deleteFileTasks = new List<Task>();
        foreach (var fileUrl in fileUrls)
        {
            var currentGuid = fileUrl.Split("/").Last();
            
            // deleteMetadataTasks.Add(database.KeyDeleteAsync(fileGuid.ToString()));
            var removeArgs = new RemoveObjectArgs()
                .WithBucket(BucketName)
                .WithObject(currentGuid);
            deleteFileTasks.Add(minio.RemoveObjectAsync(removeArgs));
        }

        // return Task.WhenAll(deleteMetadataTasks.Concat(deleteFileTasks));
        return Task.WhenAll(deleteFileTasks);
    }
}