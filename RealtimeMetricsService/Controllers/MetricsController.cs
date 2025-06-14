using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.MessageContracts;

namespace RealtimeMetricsService.Controllers;

[ApiController]
[Route("metrics")]
public class MetricsController(
    IBus bus) : ControllerBase
{
    [HttpPost("content/{id:long}")]
    public async Task<IActionResult> SendContentPageOpened([FromRoute] long id, CancellationToken cancellationToken)
    {
        await bus.Publish(new ContentPageOpenedEvent(id), cancellationToken);
        return NoContent();
    }
}