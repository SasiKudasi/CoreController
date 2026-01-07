using CoreController.Contracts;

namespace CoreController.Utils
{
    public static class CommandParser
    {
        public static string ParseCommand(string input)
        {
            return input.ToLower() switch
            {
                var c when c.Contains("включи") && c.Contains("пк") => "wake_pc",
                _ => "unknown"
            };
        }
    }
}
