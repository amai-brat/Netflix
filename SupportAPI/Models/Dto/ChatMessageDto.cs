// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SupportAPI.Models.Dto
{
    public class ChatMessageDto
    {
        public string Role { get; set; } = null!;
        public List<FileInfoDto>? Files { get; set; }
        public string Text { get; set; } = null!;
    }
}
