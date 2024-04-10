using Domain.Services.ServiceExceptions;
using Infrastucture.Options;
using Infrastucture.Services.Exceptions;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace Infrastucture.Services;

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

        try
        {
            await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(BucketName)
                .WithObject(name)
                .WithStreamData(pictureStream)
                .WithObjectSize(pictureStream.Length)
                .WithContentType(contentType)
            );
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<Stream> GetAsync(string name)
    {
        var destination = new MemoryStream();
        try
        {
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
        }
        catch (Exception ex)
        {
            throw;
        }

        return destination;
    }

    public async Task<string> GetUrlAsync(string name)
    {
        if (Uri.TryCreate(name, new UriCreationOptions(), out _))
            return name;
        
        try
        {
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
        catch (Exception ex)
        {
            throw;
        }
    }
}