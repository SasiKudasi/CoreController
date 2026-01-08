
using CoreController.WebSocket;
using Microsoft.AspNetCore.SignalR;

namespace CoreController.Commands
{
    public class RelaxModeCommand : ICommand
    {
        public string Name => "relax_mode";
        private readonly IHubContext<AgentHub> _hub;

        public RelaxModeCommand(IHubContext<AgentHub> hub)
        {
            _hub = hub;
        }

        public async Task<string> ExecuteAsync()
        {
            var cmd = new AgentCommand("relax_apps");
            await _hub.Clients.Group("home-pc").SendAsync("ReceiveCommand", cmd);
            return "Запускаю relax";
        }
    }
}
