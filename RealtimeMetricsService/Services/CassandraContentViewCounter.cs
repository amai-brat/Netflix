using Cassandra;
using RealtimeMetricsService.Helpers;
using RealtimeMetricsService.Models;

namespace RealtimeMetricsService.Services;

public class CassandraContentViewCounter(
    Cluster cluster) : IContentViewCounter
{
    public async Task AddViewsAsync(long contentId, int views, CancellationToken cancellationToken = default)
    {
        var session = await cluster.ConnectAsync();
        var ps = await session.PrepareAsync(
            $"UPDATE {Consts.CountersTableFullName} " +
            $"SET views = views + ? " +
            $"WHERE content_id = ?");
        var statement = ps.Bind((long)views, contentId);
        await session.ExecuteAsync(statement);
    }

    public async Task<long> GetViewCountAsync(long contentId, CancellationToken cancellationToken = default)
    {
        var session = await cluster.ConnectAsync();
        var ps = await session.PrepareAsync(
            $"SELECT views FROM {Consts.CountersTableFullName} " +
            $"WHERE content_id = ?");
        var statement = ps.Bind(contentId);
        var rs = await session.ExecuteAsync(statement);
        var row = rs.FirstOrDefault();
        return row?.GetValue<long>(0) ?? 0;
    }

    public async Task<List<ContentViewCount>> GetAllViewCountsAsync(CancellationToken cancellationToken = default)
    {
        var session = await cluster.ConnectAsync();
        var rs = await session.ExecuteAsync(
            new SimpleStatement($"SELECT content_id, views FROM {Consts.CountersTableFullName}"));
        return rs
            .Select(row => new ContentViewCount
            {
                ContentId = row.GetValue<long>(0),
                Views = row.GetValue<long>(1)
            })
            .ToList();
    }
}
