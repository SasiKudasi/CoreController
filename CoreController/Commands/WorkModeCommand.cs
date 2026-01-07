
using CoreController.WebSocket;
using Microsoft.AspNetCore.SignalR;

namespace CoreController.Commands
{
    public class WorkModeCommand : ICommand
    {
        public string Name => "work_mode";
        private readonly IHubContext<AgentHub> _hub;

        public WorkModeCommand(IHubContext<AgentHub> hub)
        {
            _hub = hub;
        }

        public async Task<string> ExecuteAsync()
        {
            var cmd = new AgentCommand("work_apps");
            await _hub.Clients.Group("home-pc").SendAsync("ReceiveCommand", cmd);
            return "Запускаю рабочий режим";
        }
    }
}
