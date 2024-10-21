using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupportAPI.Services;

namespace SupportAPI.Controllers
{
    [Route("supportChat/[controller]")]
    [ApiController]
    [Authorize]
    public class HistoryController (IHistoryService historyService) : ControllerBase
    {
        /// <summary>
        /// Этот метод предназначен для получения пользователем истории своего чата поддержки
        /// </summary>
        [Authorize(Roles = "user")]
        [HttpGet]
        public async Task<IActionResult> GetHistory()
        {
            var userId = long.Parse(User.FindFirst("id")!.Value);

            var chatHistory = await historyService.GetMessagesByChatSessionIdAsync(userId);

            return Ok(chatHistory);
        }

        [Authorize(Roles = "support, admin, moderator")]
        [HttpGet("{chatSessionId}")]
        public async Task<IActionResult> GetHistoryByChatSessionId(long chatSessionId)
        {
            return Ok(await historyService.GetMessagesByChatSessionIdAsync(chatSessionId));
        }
    }
}
