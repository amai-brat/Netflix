using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using SupportPermanentS3Service.Enums;
using SupportPermanentS3Service.Models.Dto;

namespace SupportPermanentS3Service.Services.Impl;

public class FileCopyService(
    [FromKeyedServices(KeyedMinios.Temporary)] IMinioClient tempMinio,
    [FromKeyedServices(KeyedMinios.Permanent)] IMinioClient permMinio,
    ILogger<FileCopyService> logger)
    : IFileCopyService
{
    public const string BucketName = "chat-files";
    public const string Policy = """
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

    public async Task<List<Uri>> CopyFilesAsync(List<Uri> fileUris,
        CancellationToken cancellationToken)
    {
        var fields = fileUris
            .Select(x => x.AbsolutePath[1..])
            .Select(FieldDto.Parse)
            .ToList();
        
        return await CopyFilesAsync(fields, cancellationToken);
    }
    
    public async Task<List<Uri>> CopyFilesAsync(List<FieldDto> fieldsToCopy,
        CancellationToken cancellationToken = default)
    {
        var uriList = new List<Uri>();
        var copyTasks = new List<Task>();

        var bucketExistsArgs = new BucketExistsArgs().WithBucket(BucketName);
        bool found = await permMinio.BucketExistsAsync(bucketExistsArgs, cancellationToken);
        if (!found)
        {
            var makeBucketArgs = new MakeBucketArgs().WithBucket(BucketName);
            var setPolicyArgs = new SetPolicyArgs().WithBucket(BucketName)
                .WithPolicy(Policy);
            
            await permMinio.MakeBucketAsync(makeBucketArgs, cancellationToken);
            await permMinio.SetPolicyAsync(setPolicyArgs, cancellationToken);
        }
        
        foreach (var (bucket, @object) in fieldsToCopy)
        {
            var guid = @object;
            var statObjArgs = new StatObjectArgs()
                .WithBucket(BucketName)
                .WithObject(guid);
            var stats = await tempMinio.StatObjectAsync(statObjArgs, cancellationToken);
            var metadata = stats.MetaData;
            
            var getObjectArgs = new GetObjectArgs()
                .WithBucket(bucket)
                .WithObject(guid)
                .WithCallbackStream(async (s, ct) =>
                {
                    var putObjectArgs = new PutObjectArgs()
                        .WithBucket(BucketName)
                        .WithObject(guid)
                        .WithHeaders(metadata)
                        .WithObjectSize(-1)
                        .WithStreamData(s);

                    await permMinio.PutObjectAsync(putObjectArgs, ct);
                });

            copyTasks.Add(tempMinio.GetObjectAsync(getObjectArgs, cancellationToken));
            var presignedGetUrl = new PresignedGetObjectArgs()
                .WithBucket(BucketName)
                .WithObject(guid)
                .WithExpiry(3600);

            var result = await permMinio.PresignedGetObjectAsync(presignedGetUrl);
            uriList.Add(new Uri(result));
            // var current = options.Value;
            //
            // uriList.Add(new UriBuilder(Uri.UriSchemeHttp, current.ExternalEndpoint,
            //     current.Port, $"file/{guid}").Uri);
        }

        await Task.WhenAll(copyTasks);
        return uriList;
    }
    
    
    public async Task GetFileAsync(Stream body, Guid guid, CancellationToken cancellationToken)
    {
        var getArgs = new GetObjectArgs()
            .WithBucket(BucketName)
            .WithObject(guid.ToString())
            .WithCallbackStream(async (s,ct) =>
            {
                await s.CopyToAsync(body, ct);
            });
        await permMinio.GetObjectAsync(getArgs, cancellationToken);
    }

    public async Task<bool> CanGetFileAsync(Guid guid, string role, int id, CancellationToken cancellationToken)
    {
        var statsArgs = new StatObjectArgs()
            .WithBucket(BucketName)
            .WithObject(guid.ToString());
        try
        {
            var resp = await permMinio.StatObjectAsync(statsArgs, cancellationToken);
            var metadata = resp.MetaData;

            if (role == "user")
            {
                return metadata["for"] == id.ToString();
            }

            return true;
        }
        catch (ObjectNotFoundException)
        {
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError("Error while getting file: {@Exception}", ex);
            throw;
        }
    }

    public async Task<Uri?> GetPresignedUriAsync(Guid guid, CancellationToken cancellationToken)
    {
        try
        {
            await permMinio.StatObjectAsync(new StatObjectArgs()
                .WithBucket(BucketName)
                .WithObject(guid.ToString()), cancellationToken);
        }
        catch (ObjectNotFoundException)
        {
            return null;
        }
        
        var presignedGetUrl = new PresignedGetObjectArgs()
            .WithBucket(BucketName)
            .WithObject(guid.ToString())
            .WithExpiry(3600);
        
        return new Uri(await permMinio.PresignedGetObjectAsync(presignedGetUrl));
    }
}