using AgentClient;
using AgentClient.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;


var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var serverUrl = config["ServerUrl"];
var agentId = config["AgentId"];


var connection = new HubConnectionBuilder()
            .WithUrl(serverUrl)
            .WithAutomaticReconnect()
            .Build();

var programsSection = config.GetSection("Programs").Get<Dictionary<string, List<string>>>();

var agentState = new AgentState();


var programs = new ProgramRegistry(programsSection);

var appLauncher = new AppLauncher(programs);
connection.On<AgentCommand>("ReceiveCommand", (cmd) =>
{
    if (agentState.AgentMode != cmd.Type)
    {
        if(!string.IsNullOrEmpty(agentState.AgentMode))
        {
            appLauncher.KillApps(agentState.AgentMode);
        }
        appLauncher.StartApps(cmd.Type);
        agentState.AgentMode = cmd.Type;
    }
    else
    {
        Console.WriteLine($"[Agent] This mode is running: {cmd.Type}");
    }
});


await connection.StartAsync();
if (connection.State == HubConnectionState.Connected)
{
    Console.WriteLine("[Agent] Successfully connected to server!");
}
else
{
    Console.WriteLine("[Agent] Connection failed or not established.");
}

await connection.InvokeAsync("Register", agentId);
await Task.Delay(-1);