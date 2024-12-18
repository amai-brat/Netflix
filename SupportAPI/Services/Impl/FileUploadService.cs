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
    // private const string Policy = """
    //                               {
    //                                   "Version": "2012-10-17",
    //                                   "Statement": [
    //                                       {
    //                                           "Effect": "Allow",
    //                                           "Principal": { "AWS": ["*"] },
    //                                           "Action": ["s3:GetObject"],
    //                                           "Resource": ["arn:aws:s3:::chat-files/*"]
    //                                       }
    //                                   ]
    //                               }
    //                               """;
    public async Task<string> UploadFileAndSaveMetadataAsync(
        Stream s,
        Dictionary<string,string> metadata,
        CancellationToken cancellationToken)
    {
        // We'll send this array to permanent s3 storage telling these files are uploaded and ready to be copied
        Guid guid = NewId.NextGuid();
        var resp = await s3Client.ListBucketsAsync(new ListBucketsRequest(){Prefix = BucketName}, cancellationToken);
        if (resp.Buckets.Count == 0)
        {
            await s3Client.PutBucketAsync(new PutBucketRequest(){BucketName = BucketName}, cancellationToken);
            // await s3Client.PutBucketPolicyAsync(new PutBucketPolicyRequest()
            // {
            //     BucketName = BucketName,
            //     Policy = Policy
            // });
        }
        var transferUtility = new TransferUtility(s3Client);
        
        try
        {
            var req = new TransferUtilityUploadRequest()
            {
                BucketName = BucketName,
                InputStream = s,
                Key = guid.ToString()
            };
            foreach (var mkv in metadata)
            {
                req.Metadata.Add(mkv.Key, mkv.Value);
            }
            await transferUtility.UploadAsync(req, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogInformation($"Error during upload: {ex.Message}");
            throw;
        }
        return s3Client.Config.ServiceURL + BucketName + "/" + guid;
    }
    
    public Task DeleteFileAndMetadataAsync(List<string> fileUrls, CancellationToken cancellationToken)
    {
        var deleteFileTasks = new List<Task>();
        foreach (var fileUrl in fileUrls)
        {
            var currentGuid = fileUrl.Split("/").Last();
            
            var removeArgs = new RemoveObjectArgs()
                .WithBucket(BucketName)
                .WithObject(currentGuid);
            deleteFileTasks.Add(minio.RemoveObjectAsync(removeArgs, cancellationToken));
        }

        return Task.WhenAll(deleteFileTasks);
    }
}