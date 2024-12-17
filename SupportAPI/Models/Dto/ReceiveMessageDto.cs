// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SupportAPI.Models.Dto
{
    public class ReceiveMessageDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public ChatMessageDto Message { get; set; } = null!;
    }
}
