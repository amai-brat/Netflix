namespace SupportAPI.Data.Entities
{
    public class SupportChatSession
    {
        /// <summary>
        /// Id - это UserId пользователя который владеет чатом
        /// </summary>
        public long Id { get; set; }

        public string? UserName { get; set; }

        public bool IsAnswered { get; set; }

        public List<SupportChatMessage>? ChatMessages { get; set; } = [];
    }
}
