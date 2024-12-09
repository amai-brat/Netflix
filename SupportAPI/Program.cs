using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Shared.MessageContracts;
using SupportAPI;
using SupportAPI.Configuration;
using SupportAPI.Extensions;
using SupportAPI.Hubs;
using SupportAPI.Models;
using SupportAPI.Models.Dto;
using SupportAPI.Services;
using SupportAPI.Services.Impl;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddOptions(builder.Configuration);
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddRedisCache();
builder.Services.AddMinio();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCorsWithFrontendPolicy(builder.Configuration);
builder.Services.AddMassTransitRabbitMq(
    builder.Configuration.GetSection("RabbitMqConfig").Get<RabbitMqConfig>()!
);

var app = builder.Build();


app.UseRouting();
app.UseCors("Frontend");

app.MapPost("test-message", [Authorize] async (IHubContext<SupportHub> hubContext,
    long sessionId, string message, HttpContext httpContext, IBus bus) =>
{
    var userId = long.Parse(httpContext.User.FindFirst("id")!.Value!);
    var senderName = httpContext.User!.Identity!.Name!;
    var role = httpContext.User!.IsInRole("support") ? "support" : "user";
    
    var chatMessageEvent = new ChatMessageEvent()
    {
        ChatSessionId = sessionId,
        SenderName = senderName,
        Role = role,
        DateTimeSent = DateTimeOffset.Now,
        SenderId = userId,
        Text = message
    };

    var receiveMessageDto = new ReceiveMessageDto()
    {
        Id = userId,
        Name = senderName,
        Message = new ChatMessageDto()
        {
            Role = role,
            Text = message
        }
    };

    await bus.Publish(chatMessageEvent);
    await hubContext.Clients.Group(sessionId.ToString()).SendAsync("ReceiveMessage", receiveMessageDto);

    SupportHub.ConnectionGroups.Where(kvp => !kvp.Value.Contains(userId.ToString()))
        .Select(kvp => kvp.Key)
        .ToList()
        // ReSharper disable once AsyncVoidLambda
        .ForEach(async (connectionId) =>
        {
            await hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", receiveMessageDto);
        });
    
    return Results.Ok();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<SupportHub>("/hub/support");
app.MapControllers();

app.Run();
