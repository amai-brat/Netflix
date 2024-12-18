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
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var chatHistory = await historyService.GetMessagesByChatSessionIdAsync(userId, token);

            return Ok(chatHistory);
        }

        [Authorize(Roles = "support, admin, moderator")]
        [HttpGet("{chatSessionId:long}/messages")]
        public async Task<IActionResult> GetHistoryByChatSessionId(long chatSessionId)
        {
            try
            {
                var authToken = Request.Headers["Authorization"];
                var token = authToken.ToString().Split(" ")[1];
                return Ok(await historyService.GetMessagesByChatSessionIdAsync(chatSessionId, token));
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
