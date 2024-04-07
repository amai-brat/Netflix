using Infrastucture.Options;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Shared;

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

    public async Task<Result> PutAsync(string name, Stream pictureStream, string contentType)
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
            return Result.Failure(Error.Failure(ex.Message));
        }

        return Result.Success();
    }

    public async Task<Result<Stream>> GetAsync(string name)
    {
        var destination = new MemoryStream();
        try
        {
            var stat = await _minioClient.StatObjectAsync(new StatObjectArgs()
                .WithBucket(BucketName)
                .WithObject(name));

            if (stat is null || stat.DeleteMarker)
            {
                return Result.Failure<Stream>(Error.Failure("Файл удалён."));
            }

            await _minioClient.GetObjectAsync(new GetObjectArgs()
                .WithBucket(BucketName)
                .WithObject(name)
                .WithCallbackStream(x => x.CopyToAsync(destination)));
        }
        catch (Exception ex)
        {
            return Result.Failure<Stream>(Error.Failure(ex.Message));
        }

        return destination;
    }

    public async Task<Result<string>> GetUrlAsync(string name)
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
                return Result.Failure<string>(Error.Failure("Файл удалён."));
            }

            return await _minioClient.PresignedGetObjectAsync(new PresignedGetObjectArgs()
                .WithBucket(BucketName)
                .WithObject(name)
                .WithExpiry(3600));
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(Error.Failure(ex.Message));
        }
    }
}