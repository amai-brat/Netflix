namespace Shared.MessageContracts;

public record FilesAndMetadatasUploaded(long SessionId ,List<Guid> FileId);