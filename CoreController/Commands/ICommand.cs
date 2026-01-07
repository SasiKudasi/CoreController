using CoreController.Contracts;

namespace CoreController.Commands
{
    public interface ICommand
    {
        string Name { get; }
        Task<string> ExecuteAsync();
    }
}
