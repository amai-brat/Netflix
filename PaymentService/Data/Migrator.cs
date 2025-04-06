using Microsoft.EntityFrameworkCore;

namespace PaymentService.Data;

public static class Migrator
{
    public static async Task MigrateAsync(IServiceProvider serviceProvider)
    {
        await using(var scope = serviceProvider.CreateAsyncScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await dbContext.Database.MigrateAsync();
        }
    }
}