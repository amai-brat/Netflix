namespace SupportAPI.Models;

public class SupportChatSessionDto
{
    /// <summary>
    /// Id - это UserId пользователя который владеет чатом
    /// </summary>
    public long Id { get; set; }

    public string? UserName { get; set; }

    public bool IsAnswered { get; set; }

    public List<ChatMessageDto> Messages { get; set; } = [];
}