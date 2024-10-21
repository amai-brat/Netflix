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
        private static readonly ConcurrentDictionary<string, string> _userIdConnection = [];
        private static readonly ConcurrentDictionary<string, List<string>> _connectionGroups = [];

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User!.FindFirst("id")!.Value!;
            var connectionId = _userIdConnection[userId];

            if (Context!.User!.IsInRole("user"))
            {
                await Groups.RemoveFromGroupAsync(connectionId, userId);

            }
            else
            {
                var groups = _connectionGroups[connectionId];

                foreach (var group in groups)
                {
                    await Groups.RemoveFromGroupAsync(connectionId, group);
                }

                _connectionGroups.TryRemove(connectionId, out _);
            }

            _userIdConnection.TryRemove(userId, out _);
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User!.FindFirst("id")!.Value;

            _userIdConnection.TryAdd(userId, Context.ConnectionId);

            if (Context.User.IsInRole("user"))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }
            else
            {
                _connectionGroups.TryAdd(Context.ConnectionId, []);
            }
        }

        public async Task SendMessage(long chatSessionId, string message)
        {
            var userId = long.Parse(Context.User?.FindFirst("id")!.Value!);
            
            if (Context!.User!.IsInRole("user") && userId != chatSessionId)
            {
                return;
            }

            var chatMessage = new ChatMessageDto()
            {
                ChatSessionId = chatSessionId,
                DateTimeSent = DateTimeOffset.Now,
                SenderId = userId,
                Text = message
            };

            await bus.Publish(chatMessage);
            await Clients.Group(userId.ToString()).SendAsync("ReceiveMessage", chatMessage);
        }

        [Authorize(Roles = "admin, moderator, support")]
        public async Task JoinUserSupportChat(long chatId)
        {
            var groupName = chatId.ToString();

            _connectionGroups[Context.ConnectionId].Add(groupName);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
