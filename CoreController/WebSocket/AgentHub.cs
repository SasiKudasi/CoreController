using Microsoft.AspNetCore.SignalR;

namespace CoreController.WebSocket;

public class AgentHub : Hub
{
    public async Task Register(string agentId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, agentId);
        Console.WriteLine($"Agent registered: {agentId}");
    }
}
