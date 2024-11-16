namespace SupportPersistentAPI.Data.Entities
{
    public class SupportChatMessage
    {
        public long Id { get; set; }

        public long SenderId { get; set; }
        public string SenderName { get; set; } = null!;
        public string Role { get; set; } = null!;

        public DateTimeOffset DateTimeSent { get; set; }
        public string Text { get; set; } = null!;

        public long ChatSessionId { get; set; }
        public SupportChatSession ChatSession { get; set; } = null!;
    }
}
