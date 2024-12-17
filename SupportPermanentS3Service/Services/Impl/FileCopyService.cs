using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using SupportPermanentS3Service.Enums;
using SupportPermanentS3Service.Options;

namespace SupportPermanentS3Service.Services.Impl;

public class FileCopyService(
    [FromKeyedServices(KeyedMinios.Temporary)] IMinioClient tempMinio,
    [FromKeyedServices(KeyedMinios.Permanent)] IMinioClient permMinio,
    IOptions<PermanentMinioOptions> options)
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
        
        foreach (var fileUri in fileUris)
        {
            var guid = fileUri.Segments.Last();
            var statObjArgs = new StatObjectArgs()
                .WithBucket(BucketName)
                .WithObject(guid);
            var stats = await tempMinio.StatObjectAsync(statObjArgs, cancellationToken);
            var metadata = stats.MetaData;
            
            var getObjectArgs = new GetObjectArgs()
                .WithBucket(fileUri.Segments[1].Replace("/",""))
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
            var current = options.Value;
            uriList.Add(new UriBuilder(Uri.UriSchemeHttp, current.ExternalEndpoint,
                current.Port, $"file/{guid}").Uri);
        }

        await Task.WhenAll(copyTasks);
        foreach (var uri in uriList)
        {
            Console.WriteLine(uri.ToString());
        }
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
        var resp = await permMinio.StatObjectAsync(statsArgs, cancellationToken);
        var metadata = resp.MetaData;
        
        if (role == "user")
        {
            return metadata["for"] == id.ToString();
        }

        return true;
    }
}