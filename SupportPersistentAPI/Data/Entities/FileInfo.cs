using Shared.MessageContracts;

namespace SupportPersistentAPI.Data.Entities;

public class FileInfo
{
    public int Id { get; set; }
    public required FileType Type { get; set; }
    public required int TypeId { get; set; }
    public FileTypeEntity TypeLookup { get; set; } = null!;
    public required string Src { get; set; }
    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
    public required string Name { get; set; }
    // public string? Metadata { get; set; }
}