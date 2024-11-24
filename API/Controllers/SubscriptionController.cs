using Application.Dto;
using Application.Features.Subscriptions.Commands.CreateSubscription;
using Application.Features.Subscriptions.Commands.DeleteSubscription;
using Application.Features.Subscriptions.Commands.EditSubscription;
using Application.Features.Subscriptions.Queries.GetAvailableContents;
using Application.Features.Subscriptions.Queries.GetSubscriptions;
using Application.Services.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize(Roles = "admin")]
[Route("admin/subscription")]
public class SubscriptionController(
    IMediator mediator)
    : ControllerBase
{
    [HttpGet("all")]
    [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
    [ProducesResponseType<List<GetSubscriptionDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllSubscriptionsAsync()
    {
        var result = await mediator.Send(new GetSubscriptionsQuery());
        return Ok(result.Dtos);
    }

    [HttpGet("contents")]
    [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> GetAvailableContentsForSubscriptionAsync()
    {
        var result = await mediator.Send(new GetAvailableContentsQuery());
        return Ok(result.Dtos);
    }
    
    [HttpPost("add")]
    [ProducesResponseType<int>(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddSubscriptionAsync(CreateSubscriptionCommand command)
    {
        var result = await mediator.Send(command);
        return Created("", result.Id);
    }

    [HttpDelete("delete/{subscriptionId:int}")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteSubscriptionAsync(int subscriptionId)
    {
        var result = await mediator.Send(new DeleteSubcriptionCommand(subscriptionId));
        return Ok(result.Id);
    }

    [HttpPut("edit")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    public async Task<IActionResult> EditSubscriptionAsync(EditSubscriptionCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result.Id);
    }
}