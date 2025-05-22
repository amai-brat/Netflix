using Grpc.Core;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Shared.MessageContracts;
using SupportAPI.Helpers;
using SupportAPI.Services.Abstractions;

namespace SupportAPI.Services.Implementations;

[Authorize]
public class SupportChatService(
    IBus bus,
    ISupportChatSessionManager sessionManager
    ) : SupportChat.SupportChatBase
{
    public override Task<ConnectResponse> Connect(ConnectRequest request, ServerCallContext context)
    {
        var userId = GetUserId(context);
        
        var sessionId = sessionManager.CreateUserSession(userId);
        
        return Task.FromResult(new ConnectResponse() { SessionId = sessionId });
    }

    public override async Task ConnectToStream(ConnectToStreamRequest request, IServerStreamWriter<SupportChatMessage> responseStream, ServerCallContext context)
    {
        var userId = GetUserId(context);
        sessionManager.BindSessionWithStream(request.SessionId, responseStream);
        
        if (!sessionManager.IsSessionBelongToUser(userId, request.SessionId))
            return;
        
        try
        {
            while (!context.CancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000, context.CancellationToken);
            }
        }
        catch (Exception)
        {
            // ignored
        }
        finally
        {
            sessionManager.RemoveUserSession(userId, request.SessionId);
        }
    }

    public override async Task<SupportChatMessageAck> SendMessage(SupportChatSendMessage request, ServerCallContext context)
    {
        var userDetails = GetUserDetails(context);
        
        if (!sessionManager.IsSessionBelongToUser(userDetails.Id, request.SessionId))
            return new SupportChatMessageAck();
        
        //User не может отправлять сообщения в чужую группу
        if (!IsInRolesOr(context, "admin", "moderator", "support") && userDetails.Id != request.GroupOwnerId)
            return new SupportChatMessageAck();

        var chatMessageEvent = CreateChatMessageEvent(request, userDetails);
        var supportChatMessage = CreateSupportChatMessage(request, userDetails);
        
        await bus.Publish(chatMessageEvent);
        await sessionManager.BroadcastMessageToUserGroupAsync(request.GroupOwnerId, supportChatMessage,[request.SessionId]);
        
        return new SupportChatMessageAck();
    }

    [Authorize(Roles = "admin, moderator, support")]
    public override Task<JoinSupportChatResponse> JoinSupportChat(JoinSupportChatRequest request, ServerCallContext context)
    {
        var userId = GetUserId(context);
        if(!sessionManager.IsSessionBelongToUser(userId, request.SessionId))
            return Task.FromResult(new JoinSupportChatResponse());
        
        sessionManager.JoinUserSessionGroup(request.GroupOwnerId, request.SessionId);
        return Task.FromResult(new JoinSupportChatResponse());
    }

    [Authorize(Roles = "admin, moderator, support")]
    public override Task<LeaveSupportChatResponse> LeaveSupportChat(LeaveSupportChatRequest request, ServerCallContext context)
    {
        var userId = GetUserId(context);
        if(!sessionManager.IsSessionBelongToUser(userId, request.SessionId))
            return Task.FromResult(new LeaveSupportChatResponse());
        
        sessionManager.LeaveUserSessionGroup(request.GroupOwnerId, request.SessionId);
        return Task.FromResult(new LeaveSupportChatResponse());
    }

    private static bool IsInRolesOr(ServerCallContext context, params string[] roles)
    {
        var httpContext = context.GetHttpContext();
        return roles.Any(role => httpContext.User.IsInRole(role));
    }
    
    private static ChatMessageEvent CreateChatMessageEvent(SupportChatSendMessage request, (long Id, string Name, string Role) userDetails)
        => new()
            {
                ChatSessionId = request.GroupOwnerId,
                SenderName = userDetails.Name,
                Role = userDetails.Role,
                DateTimeSent = DateTimeOffset.Now,
                SenderId = userDetails.Id,
                Text = request.Message,
                FileInfo = request.Files?.Select(f => new Shared.MessageContracts.FileInfo()
                {
                    Name = f.Name,
                    Src = new Uri(f.Src),
                    Type = FileTypeMapperHelper.MapFileType(f.Type)
                }).ToList()
            };
    
    private static SupportChatMessage CreateSupportChatMessage(SupportChatSendMessage request, (long Id, string Name, string Role) userDetails)
        => new()
        {
            Id = userDetails.Id,
            Name = userDetails.Name,
            Message = new SupportChatMessageBase()
            {
                Role = userDetails.Role,
                Text = request.Message,
                Files = { request.Files?.Select(f => new FileInformation()
                {
                    Name = f.Name,
                    Src = f.Src,
                    Type = f.Type
                }).ToList() }
            }
        };
    
    private static (long Id, string Name, string Role) GetUserDetails(ServerCallContext context) => 
    (
        GetUserId(context),
        GetUserName(context),
        GetUserRole(context)
    );
    
    private static long GetUserId(ServerCallContext context)
        => long.Parse(context.GetHttpContext().User.FindFirst("id")!.Value);
    
    private static string GetUserName(ServerCallContext context)
        => context.GetHttpContext().User.Identity!.Name!;
    
    private static string GetUserRole(ServerCallContext context)
        => context.GetHttpContext().User.IsInRole("support") ? "support" : "user";
}