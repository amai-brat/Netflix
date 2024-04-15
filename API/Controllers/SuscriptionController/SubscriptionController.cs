using Domain.Abstractions;
using Domain.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SuscriptionController;

[ApiController]
// [Authorize(Roles = "admin")]
[Route("admin/subscriptions")]
public class SubscriptionController(
    ISubscriptionService subscriptionService)
    : ControllerBase
{
    [HttpGet("all")]
    [ProducesResponseType<List<Subscription>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllSubscriptionsAsync()
    {
        var result = await subscriptionService.GetSubscriptionsAsync();
        
        return Ok(result);
    }

    [HttpGet("contents")]
    public async Task<IActionResult> GetAvailableContentsForSubscriptionAsync()
    {
        var result = await subscriptionService.GetContentsAsync();

        return Ok(result);
    }
    
    [HttpPost("add")]
    [ProducesResponseType<Subscription>(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddSubscriptionAsync(NewSubscriptionDto dto)
    {
        var result = await subscriptionService.AddSubscriptionAsync(dto);
        return Ok(result);
    }

    [HttpDelete("delete/{subscriptionId:int}")]
    [ProducesResponseType<Subscription>(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteSubscriptionAsync(int subscriptionId)
    {
        return Ok(await subscriptionService.DeleteSubscriptionAsync(subscriptionId));
    }

    [HttpPut("edit")]
    [ProducesResponseType<Subscription>(StatusCodes.Status200OK)]
    public async Task<IActionResult> EditSubscriptionAsync(EditSubscriptionDto dto)
    {
        return Ok(await subscriptionService.EditSubscriptionAsync(dto));
    }
}