using Hangfire;
using SupportPermanentS3Service.Services;

namespace SupportPermanentS3Service.BackgroundServices;

public class TempCleanerBackgroundService(
    IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var tempCleanerService = scope.ServiceProvider.GetService<ITempCleanerService>()!;
        
        RecurringJob.AddOrUpdate("cleaner", 
            () => tempCleanerService.CleanTempAsync(stoppingToken),
            "00 00 * * *"); // каждый день в 00:00
    }
}