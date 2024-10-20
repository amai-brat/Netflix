using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SupportAPI.Controllers
{
    [Route("supportChat/[controller]")]
    [ApiController]
    [Authorize]
    public class HistoryController : ControllerBase
    {
        /// <summary>
        /// This method returns Support chat history only for users, because they only have one Chat Session.
        /// </summary>
        [Authorize(Roles = "user")]
        [HttpGet]
        public IEnumerable<string> GetHistory()
        {
            var userId = User.FindFirst("id")!.Value;



            return new string[] { "value1", "value2" };
        }

        [Authorize(Roles = "support, admin, moderator")]
        [HttpGet("{chatSessionId}")]
        public IEnumerable<string> GetHistoryByChatSessionId(long chatSessionId)
        {
            return new string[] { "value1" };
        }
    }
}
