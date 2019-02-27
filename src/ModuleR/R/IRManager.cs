using System.Threading.Tasks;

namespace ModuleR.R
{
    public interface IRManager
    {
        Task<bool> InitialiseAsync();

        bool IsRRunning { get; }

        Task<bool> GenerateGgplotAsync(string ggplotCommand);
    }
}