using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupportPersistentAPI.Services;

namespace SupportPersistentAPI.Controllers
{
    [Route("support/chats")]
    [ApiController]
    [Authorize]
    public class HistoryController(IHistoryService historyService) : ControllerBase
    {
        /// <summary>
        /// Этот метод предназначен для получения пользователем истории своего чата поддержки
        /// </summary>
        [Authorize(Roles = "user")]
        [HttpGet("user/messages")]
        public async Task<IActionResult> GetHistory()
        {
            var userId = long.Parse(User.FindFirst("id")!.Value);

            var chatHistory = await historyService.GetMessagesByChatSessionIdAsync(userId);

            return Ok(chatHistory);
        }

        [Authorize(Roles = "support, admin, moderator")]
        [HttpGet("{chatSessionId:long}/messages")]
        public async Task<IActionResult> GetHistoryByChatSessionId(long chatSessionId)
        {
            try
            {
                return Ok(await historyService.GetMessagesByChatSessionIdAsync(chatSessionId));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "support, admin, moderator")]
        [HttpGet("unanswered")]
        public async Task<IActionResult> GetUnansweredChats()
        {
            return Ok(await historyService.GetUnansweredChatsAsync());
        }
    }
}
