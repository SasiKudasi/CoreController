

using System.Windows.Input;

namespace AgentClient;

public class ProgramRegistry
{
    private readonly Dictionary<string, List<string>> _programs;
    public ProgramRegistry(Dictionary<string, List<string>> programs)
    {
        _programs = programs;

    }

    public IEnumerable<string> GetPrograms(string key) =>
         _programs.TryGetValue(key, out var list) ? list : Enumerable.Empty<string>();

}
