using Infrastructure.Options;
using Infrastucture.Services;
using Infrastucture.Services.Exceptions;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace Infrastructure.Services;

public class ProfilePicturesProvider : IProfilePicturesProvider
{
    private const string BucketName = "avatars";
    private readonly IMinioClient _minioClient;

    public ProfilePicturesProvider(IOptionsMonitor<MinioOptions> optionsMonitor)
    {
        var minioOptions = optionsMonitor.CurrentValue;
        _minioClient = new MinioClient()
            .WithEndpoint(minioOptions.Endpoint)
            .WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey)
            .Build();
    }

    public async Task PutAsync(string name, Stream pictureStream, string contentType)
    {
        var found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(BucketName));
        if (!found)
        {
            await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(BucketName));
        }

        await _minioClient.PutObjectAsync(new PutObjectArgs()
            .WithBucket(BucketName)
            .WithObject(name)
            .WithStreamData(pictureStream)
            .WithObjectSize(pictureStream.Length)
            .WithContentType(contentType)
        );
    }

    public async Task<Stream> GetAsync(string name)
    {
        var destination = new MemoryStream();
        var stat = await _minioClient.StatObjectAsync(new StatObjectArgs()
            .WithBucket(BucketName)
            .WithObject(name));

        if (stat is null || stat.DeleteMarker)
        {
            throw new ProfilePictureProviderArgumentException(ProviderErrorMessages.FileDeleted, nameof(name));
        }

        await _minioClient.GetObjectAsync(new GetObjectArgs()
            .WithBucket(BucketName)
            .WithObject(name)
            .WithCallbackStream(x => x.CopyToAsync(destination)));

        return destination;
    }

    public async Task<string> GetUrlAsync(string name)
    {
        if (Uri.TryCreate(name, new UriCreationOptions(), out _))
            return name;

        var stat = await _minioClient.StatObjectAsync(new StatObjectArgs()
            .WithBucket(BucketName)
            .WithObject(name));

        if (stat is null || stat.DeleteMarker)
        {
            throw new ProfilePictureProviderArgumentException(ProviderErrorMessages.FileDeleted, nameof(name));
        }

        return await _minioClient.PresignedGetObjectAsync(new PresignedGetObjectArgs()
            .WithBucket(BucketName)
            .WithObject(name)
            .WithExpiry(3600));
    }
}