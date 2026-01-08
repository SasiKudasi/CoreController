

using System.Collections.Concurrent;
using System.Diagnostics;

namespace AgentClient;

public class AppLauncher
{
    private readonly ConcurrentDictionary<string, Process> _runningProcesses = new();
    private readonly ProgramRegistry _programRegistry;
    public AppLauncher(ProgramRegistry programRegistry)
    {
        _programRegistry = programRegistry;
    }

    public void StartApps(string cmd)
    {
        var paths = _programRegistry.GetPrograms(cmd);

        foreach (var path in paths)
        {
            if (_runningProcesses.ContainsKey(path))
            {
                Console.WriteLine($"[Agent] {path} already runnign, skiping....");
                continue;
            }

            Task.Run(() =>
            {
                try
                {
                    var process = Process.Start(new ProcessStartInfo
                    {
                        FileName = path,
                        UseShellExecute = true,
                        WindowStyle = ProcessWindowStyle.Normal,
                        CreateNoWindow = false
                    });
                    if (process != null)
                        _runningProcesses.TryAdd(path, process);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Agent] Failed to start {path}: {ex.Message}");
                }
            });

        }
    }


    public void KillApps(string cmd)
    {
        var paths = _programRegistry.GetPrograms(cmd);

        foreach (var path in paths)
        {
            if (_runningProcesses.TryRemove(path, out var process))
            {
                try
                {
                    if (!process.HasExited)
                        process.Kill(true);

                    Console.WriteLine($"[Agent] Killed app: {path}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Agent] Failed to kill {path}: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"[Agent] App {path} is not running.");
            }
        }
    }
}
