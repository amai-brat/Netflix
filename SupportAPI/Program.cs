using Amazon.S3;
using Microsoft.Extensions.Options;
using SupportAPI;
using SupportAPI.Configuration;
using SupportAPI.Extensions;
using SupportAPI.Options;
using SupportAPI.Services.Abstractions;
using SupportAPI.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddOptions(builder.Configuration);
builder.Services.AddGrpc();
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithBearer();
builder.Services.AddAuthorization();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddSingleton<ISupportChatSessionManager, SupportChatSessionManager>();
builder.Services.AddSingleton<AmazonS3Client>(sp =>
{
    var minioOptions = sp.GetRequiredService<IOptions<MinioOptions>>().Value;
    
    var config = new AmazonS3Config
    {
        ServiceURL = new UriBuilder(Uri.UriSchemeHttp, minioOptions.ExternalEndpoint, minioOptions.Port).Uri.ToString(),
        ForcePathStyle = true,
        UseHttp = true
    };
    return new AmazonS3Client(minioOptions.AccessKey,
        minioOptions.SecretKey,
        config);
});
builder.Services.AddRedisCache();
builder.Services.AddMinio();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCorsWithFrontendPolicy(builder.Configuration);
builder.Services.AddMassTransitRabbitMq(
    builder.Configuration.GetSection("RabbitMqConfig").Get<RabbitMqConfig>()!
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
app.MapGrpcService<SupportChatService>();

app.MapControllers();

app.Run();
