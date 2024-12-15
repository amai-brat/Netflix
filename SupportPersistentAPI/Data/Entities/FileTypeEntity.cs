using Shared.MessageContracts;

namespace SupportPersistentAPI.Data.Entities;

public class FileTypeEntity
{
    public int Id { get; set; }
    public required string Type { get; set; }
}