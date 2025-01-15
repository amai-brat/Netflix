using Microsoft.EntityFrameworkCore;
using SupportPermanentS3Service.Data.Repositories;
using SupportPermanentS3Service.Repositories;

namespace SupportPermanentS3Service.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(builder =>
        {
            builder.UseNpgsql(configuration["Database:ConnectionString"]);
            builder.UseSnakeCaseNamingConvention();
        });
        
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IMetadataRepository, MetadataRepository>();
        services.AddScoped<IMetadataValueRepository, MetadataValueRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}