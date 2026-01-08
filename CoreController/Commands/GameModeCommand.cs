
using CoreController.WebSocket;
using Microsoft.AspNetCore.SignalR;

namespace CoreController.Commands
{
    public class GameModeCommand : ICommand
    {
        public string Name => "game_mode";
        private readonly IHubContext<AgentHub> _hub;

        public GameModeCommand(IHubContext<AgentHub> hub)
        {
            _hub = hub;
        }
        public async Task<string> ExecuteAsync()
        {
            var cmd = new AgentCommand("game_apps");
            await _hub.Clients.Group("home-pc").SendAsync("ReceiveCommand", cmd);
            return "Запускаю game mode";
        }
    }
}
