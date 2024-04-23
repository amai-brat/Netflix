using Application.Dto;
using Application.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SuscriptionController;

[ApiController]
[Authorize(Roles = "admin")]
[Route("admin/subscription")]
public class SubscriptionController(
    ISubscriptionService subscriptionService)
    : ControllerBase
{
    [HttpGet("all")]
    [ProducesResponseType<List<AdminSubscriptionsDto>>(StatusCodes.Status200OK)]
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
    [ProducesResponseType<int>(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddSubscriptionAsync(NewSubscriptionDto dto)
    {
        var result = await subscriptionService.AddSubscriptionAsync(dto);
        return Created("", result.Id);
    }

    [HttpDelete("delete/{subscriptionId:int}")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteSubscriptionAsync(int subscriptionId)
    {
        var result = await subscriptionService.DeleteSubscriptionAsync(subscriptionId);
        return Ok(result.Id);
    }

    [HttpPut("edit")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    public async Task<IActionResult> EditSubscriptionAsync(EditSubscriptionDto dto)
    {
        var result = await subscriptionService.EditSubscriptionAsync(dto);
        return Ok(result.Id);
    }
}