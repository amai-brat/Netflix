using RealtimeMetricsService.Models;

namespace RealtimeMetricsService.Services;

public interface IContentViewCounter
{
    Task AddViewsAsync(long contentId, int views, CancellationToken cancellationToken = default);
    Task<long> GetViewCountAsync(long contentId, CancellationToken cancellationToken = default);
    Task<List<ContentViewCount>> GetAllViewCountsAsync(CancellationToken cancellationToken = default);
}