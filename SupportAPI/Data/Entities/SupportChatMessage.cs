namespace SupportAPI.Data.Entities
{
    public class SupportChatMessage
    {
        public long Id { get; set; }
        public long SenderId { get; set; }
        public DateTimeOffset DateTimeSent { get; set; }
        public string Text { get; set; } = null!;

        public long ChatSessionId { get; set; }
        public SupportChatSession ChatSession { get; set; } = null!;
    }
}
