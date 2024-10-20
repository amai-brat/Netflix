using Microsoft.EntityFrameworkCore;

namespace SupportAPI.Data.Extensions
{
    public static class DbRegisterExt
    {
        public static IServiceCollection AddDbContext(this IServiceCollection serviceCollection,
        IConfiguration configuration)
        {
            

            return serviceCollection.AddDbContext<AppDbContext>(builder =>
            {
                builder.UseNpgsql(configuration["Database:ConnectionString"]);
                builder.UseSnakeCaseNamingConvention();
            });
        }
    }
}
