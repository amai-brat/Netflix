using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SupportAPI.Models;
using System.Collections.Concurrent;

namespace SupportAPI.Hubs
{
    [Authorize]
    public class SupportHub(IBus bus): Hub
    {
        private static readonly ConcurrentDictionary<string, string> UserIdConnection = [];
        private static readonly ConcurrentDictionary<string, List<string>> ConnectionGroups = [];

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User!.FindFirst("id")!.Value;
            var connectionId = UserIdConnection[userId];

            if (!IsInRolesOr("admin", "moderator", "support"))
            {
                await Groups.RemoveFromGroupAsync(connectionId, userId);
            }
            else
            {
                var groups = ConnectionGroups[connectionId];

                foreach (var group in groups)
                {
                    await Groups.RemoveFromGroupAsync(connectionId, group);
                }

                ConnectionGroups.TryRemove(connectionId, out _);
            }

            UserIdConnection.TryRemove(userId, out _);
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User!.FindFirst("id")!.Value;

            UserIdConnection.TryAdd(userId, Context.ConnectionId);

            if (!IsInRolesOr("admin", "moderator", "support"))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }
            else
            {
                ConnectionGroups.TryAdd(Context.ConnectionId, []);
            }
        }

        public async Task SendMessage(long chatSessionId, string message)
        {
            var userId = long.Parse(Context.User?.FindFirst("id")!.Value!);
            var senderName = Context.User!.Identity!.Name!;
            var role = Context.User!.IsInRole("support") ? "support" : "user";

            if (!IsInRolesOr("admin", "moderator", "support") && userId != chatSessionId)
            {
                return;
            }

            var chatMessageEvent = new ChatMessageEvent()
            {
                ChatSessionId = chatSessionId,
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
            await Clients.OthersInGroup(chatSessionId.ToString()).SendAsync("ReceiveMessage", receiveMessageDto);

            if (!IsInRolesOr("admin", "moderator", "support"))
            {
                ConnectionGroups.Where(kvp => !kvp.Value.Contains(userId.ToString()))
                                .Select(kvp => kvp.Key)
                                .ToList()
                                // ReSharper disable once AsyncVoidLambda
                                .ForEach(async (connectionId) =>
                                {
                                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", receiveMessageDto);
                                });
            }
        }

        [Authorize(Roles = "admin, moderator, support")]
        public async Task JoinUserSupportChat(long chatId)
        {
            var groupName = chatId.ToString();

            ConnectionGroups[Context.ConnectionId].Add(groupName);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
        
        [Authorize(Roles = "admin, moderator, support")]
        public async Task LeaveAllUserSupportChat()
        {
            foreach (var group in ConnectionGroups[Context.ConnectionId])
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
            }
            
            ConnectionGroups[Context.ConnectionId] = [];
        }

        private bool IsInRolesOr(params string[] roles)
        {
            foreach (var role in roles)
            {
                if (Context.User!.IsInRole(role))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
