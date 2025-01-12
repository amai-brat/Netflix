using Microsoft.EntityFrameworkCore;
using SupportPermanentS3Service.BackgroundServices;
using SupportPermanentS3Service.Data;
using SupportPermanentS3Service.Extensions;
using SupportPermanentS3Service.Services;
using SupportPermanentS3Service.Services.Impl;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddOptions(builder.Configuration);
builder.Services.AddScoped<IFileCopyService, FileCopyService>();
builder.Services.AddScoped<IMetadataCopyService, MetadataCopyService>();
builder.Services.AddScoped<ITempToPermanentCopierService, TempToPermanentCopierService>();
builder.Services.AddScoped<ITempCleanerService, TempCleanerService>();
builder.Services.AddControllers();
builder.Services.AddSwaggerGenWithBearer();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddRedisCache();
builder.Services.AddMinios();
builder.Services.AddHangfire(builder.Configuration);
builder.Services.AddData(builder.Configuration);

builder.Services.AddHostedService<TempToPermanentCopierBackgroundService>();
builder.Services.AddHostedService<TempCleanerBackgroundService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await using (var scope = app.Services.CreateAsyncScope())
{
    await Task.Delay(1000);

    var dbContext = scope.ServiceProvider.GetService<AppDbContext>();
    if ((await dbContext!.Database.GetPendingMigrationsAsync()).Any())
    {
        await dbContext.Database.MigrateAsync();
    }
}

app.UseAuthorization();

app.MapControllers();

app.Run();