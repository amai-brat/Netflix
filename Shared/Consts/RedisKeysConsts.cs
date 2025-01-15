namespace Shared.Consts;

public static class RedisKeysConsts
{
    /// <summary>
    /// Хеш-таблица для временных метаданных
    /// </summary>
    public const string MetadataKey = "metadata";
    
    /// <summary>
    /// Хеш-таблица для счётчиков загрузки файла на сервер
    /// </summary>
    public const string CountersKey = "counters";
    
    /// <summary>
    /// Хеш-таблица для счётчиков, которые не могут быть пока удален (возможно произошла где-то ошибка)
    /// </summary>
    public const string SuspiciosCountersKey = "suscounters"; 
}