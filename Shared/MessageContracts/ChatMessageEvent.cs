
namespace Shared.MessageContracts
{
    public class ChatMessageEvent
    {
        public long SenderId { get; set; }
        public string SenderName { get; set; } = null!;
        public string Role { get; set; } = null!;
        public long ChatSessionId { get; set; }
        public string Text { get; set; } = null!;
        public DateTimeOffset DateTimeSent { get; set; }
        public List<FileInfo>? FileInfo { get; set; }
    }

    public class FileInfo
    {
        public FileType Type { get; set; }
        public required Uri Src { get; set; }
        public required string Name { get; set; }
        // public string? Metadata { get; set; }
    }
    public enum FileType
    {
        Image = 1,
        Audio,
        Video,
        Document,
        Other
    }
}
