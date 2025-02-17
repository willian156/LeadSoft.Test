using LeadSoft.Test.Models.DTO.Anime;
using Microsoft.AspNetCore.SignalR;

namespace LeadSoft.Test.Hubs
{
    public class ProcessingDataHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ConnectionListener", $"{Context.ConnectionId} entrou no WebSocket!");
        }
        public async Task ProcessData(string groupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
            await Clients.Caller.SendAsync("GroupJoined", groupId);
        }

        public async Task RemoveGroupMembers(string groupId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
            await Clients.Caller.SendAsync("Group Left", groupId);
        }
    }
}
