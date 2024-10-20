namespace SupportAPI.Data.Entities
{
    public class SupportChatSession
    {
        public long Id { get; set; }
        public long UserId { get; set; }

        public List<SupportChatMessage>? Messages { get; set; }
    }
}
