using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SupportAPI.Hubs
{
    [Authorize]
    public class SupportHub: Hub
    {

    }
}
