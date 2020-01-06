using System.Threading.Tasks;
using TNCode.Core.Data;

namespace TNCodeApp.R
{
    public interface IRManager
    {
        Task<bool> InitialiseAsync();
        bool IsRRunning { get; }
        Task<bool> GenerateGgplotAsync(string ggplotCommand);
        Task<bool> DataSetToRAsDataFrameAsync(DataSet data);
        Task<string> RHomeFromConnectedRAsync();
        Task<string> RPlatformFromConnectedRAsync();
        Task<string> RVersionFromConnectedRAsync();
    }
}