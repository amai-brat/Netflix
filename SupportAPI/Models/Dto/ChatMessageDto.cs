// ReSharper disable UnusedAutoPropertyAccessor.Global

using SupportAPI.Models.Abstractions;

namespace SupportAPI.Models.Dto
{
    public class ChatMessageDto
    {
        public string Role { get; set; } = null!;
        public List<IDownloadableWithFileType>? Files { get; set; }
        public string Text { get; set; } = null!;
    }
}
