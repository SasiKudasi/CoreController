namespace CoreController.Commands
{
    public class CommandRegistry
    {
        private readonly Dictionary<string, ICommand> _commands;
        public CommandRegistry(IEnumerable<ICommand> commands)
        {
            _commands = new Dictionary<string, ICommand>();
            foreach (var cmd in commands)
            {
                _commands[cmd.Name] = cmd;
            }
        }


        public ICommand? GetCommand(string name)
        {
            _commands.TryGetValue(name, out var command);
            return command;
        }
    }
}
