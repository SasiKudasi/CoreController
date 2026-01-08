using CoreController.Contracts;

namespace CoreController.Utils
{
    public static class CommandParser
    {
        public static string ParseCommand(string input) => input.ToLower() switch
        {
            var c when c.Contains("включи") && c.Contains("пк") => "wake_pc",
            var c when c.Contains("рабочий") && c.Contains("режим") => "work_mode",
            var c when c.Contains("гейм") && c.Contains("мод") => "game_mode",
            var c when c.Contains("релакс") && c.Contains("мод") => "relax_mode",
            _ => "unknown"
        };
    }
}
