using SupportPermanentS3Service.BackgroundServices;
using SupportPermanentS3Service.Extensions;
using SupportPermanentS3Service.Services;
using SupportPermanentS3Service.Services.Impl;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddOptions(builder.Configuration);
builder.Services.AddScoped<IFileCopyService, FileCopyService>();
builder.Services.AddScoped<IMetadataCopyService, MetadataCopyService>();
builder.Services.AddControllers();
builder.Services.AddSwaggerGenWithBearer();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddRedisCache();
builder.Services.AddMinios();
builder.Services.AddHangfire(builder.Configuration);

builder.Services.AddHostedService<TempToPermanentCopierService>();
builder.Services.AddHostedService<TempCleanerService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();