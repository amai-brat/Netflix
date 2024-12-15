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
    public async Task<List<Uri>> CopyFilesAsync(List<Uri> fileGuids,
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
        
        foreach (var fileGuid in fileGuids)
        {
            var getObjectArgs = new GetObjectArgs()
                .WithBucket(fileGuid.Segments[1].Replace("/",""))
                .WithObject(fileGuid.Segments.Last())
                .WithCallbackStream(async (s, ct) =>
                {
                    var putObjectArgs = new PutObjectArgs()
                        .WithBucket(BucketName)
                        .WithObject(fileGuid.Segments.Last())
                        .WithObjectSize(-1)
                        .WithStreamData(s);

                    await permMinio.PutObjectAsync(putObjectArgs, ct);
                });

            copyTasks.Add(tempMinio.GetObjectAsync(getObjectArgs, cancellationToken));
            var current = options.Value;
            uriList.Add(new UriBuilder(Uri.UriSchemeHttp, current.ExternalEndpoint,
                current.Port, $"{BucketName}/{fileGuid.Segments.Last()}").Uri);
        }

        await Task.WhenAll(copyTasks);
        
        return uriList;
    }

}