using System.Threading.Tasks;

namespace ModuleR.R
{
    public interface IRManager
    {
        Task<bool> InitialiseAsync();
    }
}