using System.Net;
using System.Web;
using Application.Services.Abstractions;
using Infrastructure.Options;
using Infrastructure.Providers.Abstractions;
using Infrastructure.Providers.Exceptions;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace Infrastructure.Providers.Implementations;

public class ProfilePicturesProvider : IProfilePicturesProvider
{
    private const string BucketName = "avatars";
    private readonly IMinioClient _minioClient;

    public ProfilePicturesProvider(IOptionsMonitor<MinioOptions> optionsMonitor)
    {
        var minioOptions = optionsMonitor.CurrentValue;
        // тут происходит костыль: 
        // невозможно обратиться внутри контейнера на локалхост, потому что это локалхост контейнера, а не компьютера
        // PresignedGetObjectAsync даёт ссылку, в которой нельзя менять хост
        // 
        // по идее, если minio в отдельном сервере, этого костыля быть не должно
        _minioClient = new MinioClient()
            .WithEndpoint(minioOptions.ExternalEndpoint)
            .WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey)
            .WithProxy(new WebProxy(minioOptions.Endpoint, 9000)) 
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