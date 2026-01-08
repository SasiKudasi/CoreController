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



var programs = new ProgramRegistry(programsSection);
connection.On<AgentCommand>("ReceiveCommand", (cmd) =>
{
    var paths = programs.GetPrograms(cmd.Type);
    if (paths is not null)
    {
        foreach (var path in paths)
        {
            Task.Run(() =>
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = path,
                        UseShellExecute = true,
                        WindowStyle = ProcessWindowStyle.Normal,
                        CreateNoWindow = false
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Agent] Failed to start {path}: {ex.Message}");
                }
            });

        }
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
