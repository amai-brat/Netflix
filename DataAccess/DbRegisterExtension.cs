using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess
{
	public static class DbRegisterExtension
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
