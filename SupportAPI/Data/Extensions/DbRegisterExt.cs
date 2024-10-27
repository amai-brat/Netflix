using Microsoft.EntityFrameworkCore;
using SupportAPI.Data.Repositories.Implementations;
using SupportAPI.Data.Repositories.Interfaces;

namespace SupportAPI.Data.Extensions
{
    public static class DbRegisterExt
    {
        public static IServiceCollection AddDbContext(this IServiceCollection serviceCollection,
        IConfiguration configuration)
        {
            serviceCollection.AddScoped<ISupportChatMessageRepository, SupportChatMessageRepository>();
            serviceCollection.AddScoped<ISupportChatSessionRepository, SupportChatSessionRepository>();
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();

            return serviceCollection.AddDbContext<AppDbContext>(builder =>
            {
                builder.UseNpgsql(configuration["Database:ConnectionString"]);
                builder.UseSnakeCaseNamingConvention();
            });
        }
    }
}
