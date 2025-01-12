using Hangfire;
using SupportPermanentS3Service.Services;

namespace SupportPermanentS3Service.BackgroundServices;

public class TempToPermanentCopierBackgroundService(
    IServiceProvider serviceProvider
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var copier = scope.ServiceProvider.GetService<ITempToPermanentCopierService>()!;
        
        RecurringJob.AddOrUpdate("copier", 
            () => copier.CheckCountersAsync(stoppingToken), 
            Cron.Minutely);
    }
}