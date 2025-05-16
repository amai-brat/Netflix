using DataAccess;
using Infrastructure.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services;

public static class Migrator
{
    public static async Task MigrateAsync(IServiceProvider serviceProvider)
    {
        await using (var scope = serviceProvider.CreateAsyncScope())
        {
            await Task.Delay(1000);
    
            var dbContext = scope.ServiceProvider.GetService<AppDbContext>();
            if (dbContext!.Database.IsRelational())
            {
                await dbContext.Database.MigrateAsync();
            }

            var identityDbContext = scope.ServiceProvider.GetService<IdentityDbContext>();
            if (identityDbContext!.Database.IsRelational())
            {
                await identityDbContext.Database.MigrateAsync();
            }
        }
    }
}