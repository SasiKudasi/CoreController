namespace CoreController.Commands
{
    public class CommandRegistry
    {
        private readonly Dictionary<string, ICommand> _commands = new();
        public CommandRegistry()
        {
            var wake = new WakeOnLanCommand();
            _commands[wake.Name] = wake;
        }


        public ICommand? GetCommand(string name)
        {
            _commands.TryGetValue(name, out var command);
            return command;
        }
    }
}
