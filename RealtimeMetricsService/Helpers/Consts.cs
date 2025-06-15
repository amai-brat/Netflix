namespace RealtimeMetricsService.Helpers;

public static class Consts
{
    public const string KeySpace = "netflix";
    public const string CountersTableName = "view_counters";
    public static string CountersTableFullName => $"{KeySpace}.{CountersTableName}";
}