namespace SupportAPI.Models
{
    public class ChatMessageDto
    {
        public long SenderId { get; set; }
        public long ChatSessionId { get; set; }
        public string Text { get; set; } = null!;
        public DateTimeOffset DateTimeSent { get; set; }
    }
}
