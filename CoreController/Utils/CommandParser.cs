using CoreController.Contracts;

namespace CoreController.Utils
{
    public static class CommandParser
    {
        public static string ParseCommand(string input) => input.ToLower() switch
        {
            var c when c.Contains("включи") && c.Contains("пк") => "wake_pc",
            var c when c.Contains("рабочий") && c.Contains("режим") => "work_mode",
            var c when c.Contains("game") && c.Contains("mode") => "game_mode",
            var c when c.Contains("relax") && c.Contains("mode") => "relax_mode",
            _ => "unknown"
        };
    }
}
