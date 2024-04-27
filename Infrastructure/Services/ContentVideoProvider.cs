using System.Text.Unicode;
using Application.Services.Abstractions;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace Infrastructure.Services;

public class ContentVideoProvider: IContentVideoProvider
{
    private const string BucketName = "content";
    private readonly IMinioClient _minioClient;

    public ContentVideoProvider(IOptionsMonitor<MinioOptions> optionsMonitor)
    {
        var minioOptions = optionsMonitor.CurrentValue;
        _minioClient = new MinioClient()
            .WithEndpoint(minioOptions.Endpoint)
            .WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey)
            .Build();
        
    }
    public async Task PutAsync(string name, Stream videoStream, string contentType)
    {
        var found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(BucketName));
        if (!found)
        {
            await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(BucketName));
        }
        
        PutObjectArgs putObjectArgs = new PutObjectArgs()
            .WithBucket(BucketName)
            .WithObject(name)
            .WithContentType(contentType)
            .WithObjectSize(videoStream.Length)
            .WithStreamData(videoStream); 
        
        await _minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
    }

    public async Task<Stream> GetAsync(string name)
    {
        var data = new MemoryStream();
        StatObjectArgs statObjectArgs = new StatObjectArgs()
            .WithBucket(BucketName)
            .WithObject(name);
        // checks if object exists & we have permissions to access it
        // otherwise throws an exception
        await _minioClient.StatObjectAsync(statObjectArgs);

        GetObjectArgs getObjectArgs = new GetObjectArgs()
            .WithBucket(BucketName)
            .WithObject(name)
            .WithCallbackStream(async stream =>
            {
                await stream.CopyToAsync(data);
            });
        await _minioClient.GetObjectAsync(getObjectArgs);
        data.Position = 0;
        return data;
    }

}