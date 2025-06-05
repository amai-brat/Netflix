using Cassandra;

namespace RealtimeMetricsService.Helpers;

public static class Database
{
    public static async Task CreateViewCounterTableIfNotExists(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var cluster = scope.ServiceProvider.GetRequiredService<Cluster>();
        
        await CreateKeyspaceIfNotExists(cluster);
        
        var session = await cluster.ConnectAsync(Consts.KeySpace);
        await session.ExecuteAsync(
            new SimpleStatement(
                $"CREATE TABLE IF NOT EXISTS {Consts.CountersTableFullName}(" +
                $"content_id bigint, " +
                $"views counter, " +
                $"primary key ((content_id)))"));
    }

    private static async Task CreateKeyspaceIfNotExists(Cluster cluster)
    {
        var session = await cluster.ConnectAsync(); 
        await session.ExecuteAsync(
            new SimpleStatement(
                $"CREATE KEYSPACE IF NOT EXISTS {Consts.KeySpace} " +
                $"WITH REPLICATION = {{'class':'SimpleStrategy', 'replication_factor' : 1}};"
                ));
    }
}