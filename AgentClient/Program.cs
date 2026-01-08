using AgentClient;
using AgentClient.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;

internal static class Program
{
    private static NotifyIcon _trayIcon;
    private static HubConnection _connection;
    private static AgentState _agentState;
    private static AppLauncher _appLauncher;

    [STAThread]
    private static void Main()
    {
        try
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _trayIcon = new NotifyIcon()
            {
                Icon = new Icon("Resources/favicon.ico"),
                Text = "Agent Client: Connecting...",
                Visible = true
            };

            var menu = new ContextMenuStrip();
            var statusItem = new ToolStripMenuItem("Status: Connecting...");
            var workMode = new ToolStripMenuItem("Режим работы: Нет.");
            menu.Items.Add(workMode);
            menu.Items.Add(statusItem);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("Выход", null, (s, e) => Application.Exit());
            _trayIcon.ContextMenuStrip = menu;

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var serverUrl = config["ServerUrl"];
            var agentId = config["AgentId"];
            var programsSection = config.GetSection("Programs").Get<Dictionary<string, List<string>>>();

            _agentState = new AgentState();
            var programs = new ProgramRegistry(programsSection);
            _appLauncher = new AppLauncher(programs);

            _connection = new HubConnectionBuilder()
                .WithUrl(serverUrl)
                .WithAutomaticReconnect()
                .Build();

            _connection.Reconnecting += error =>
            {
                _trayIcon.Text = "Agent Client: Reconnecting...";
                statusItem.Text = "Status: Reconnecting...";
                return Task.CompletedTask;
            };

            _connection.Reconnected += connectionId =>
            {
                _trayIcon.Text = "Agent Client: Connected";
                statusItem.Text = "Status: Connected";
                NotificationManager.ShowToastNotification("Agent Client", "Reconnected to server!");
                return Task.CompletedTask;
            };

            _connection.Closed += error =>
            {
                _trayIcon.Text = "Agent Client: Disconnected";
                statusItem.Text = "Status: Disconnected";
                NotificationManager.ShowToastNotification("Agent Client", "Disconnected from server.");
                return Task.CompletedTask;
            };

            _connection.On<AgentCommand>("ReceiveCommand", cmd =>
            {
                if (_agentState.AgentMode != cmd.Type)
                {
                    NotificationManager.ShowToastNotification("Agent Client", $"Starting mode: {cmd.Type}");
                    if (!string.IsNullOrEmpty(_agentState.AgentMode))
                        _appLauncher.KillApps(_agentState.AgentMode);
                    _appLauncher.StartApps(cmd.Type);
                    _agentState.AgentMode = cmd.Type;
                    workMode.Text = $"Режим работы: {_agentState.AgentMode}";

                }
            });

            Task.Run(async () => await ConnectWithRetries(_connection, agentId, statusItem));

            Application.Run();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка запуска агента: {ex.Message}", "AgentClient", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
    }

    private static async Task ConnectWithRetries(HubConnection connection, string agentId, ToolStripMenuItem item, int maxRetries = 10, int delaySeconds = 5)
    {
        int attempt = 0;
        while (attempt < maxRetries)
        {
            try
            {
                await connection.StartAsync();
                if (connection.State == HubConnectionState.Connected)
                {
                    _trayIcon.Text = "Agent Client: Connected";
                    item.Text = "Status: Connected";
                    NotificationManager.ShowToastNotification("Agent Client", "Connected to server!");
                    await connection.InvokeAsync("Register", agentId);
                    return;
                }
            }
            catch
            {
            }

            attempt++;
            _trayIcon.Text = $"Agent Client: Connecting... (Attempt {attempt})";
            await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
        }

        _trayIcon.Text = "Agent Client: Failed to connect";
        NotificationManager.ShowToastNotification("Agent Client", "Failed to connect after multiple attempts.");
    }
}
