using System.Net;
using System.Web;
using Infrastructure.Enums;
using Infrastructure.Options;
using Infrastructure.Providers.Abstractions;
using Infrastructure.Providers.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace Infrastructure.Providers.Implementations;

public class ProfilePicturesProvider : IProfilePicturesProvider
{
    private const string BucketName = "avatars";
    private readonly IMinioClient _minioClient;

    public ProfilePicturesProvider([FromKeyedServices(KeyedServices.Avatar)] IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }

    public async Task PutAsync(string name, Stream pictureStream, string contentType)
    {
        // if bucket does not exist - create
        
        var found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(BucketName));
        if (!found)
        {
            await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(BucketName));
        }
        
        // if obj exists - delete
        
        var stat = await _minioClient.StatObjectAsync(new StatObjectArgs()
            .WithBucket(BucketName)
            .WithObject(name));

        if (stat is not null && !stat.DeleteMarker)
        {
            await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(BucketName)
                .WithObject(name));
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