using System.ComponentModel;

namespace ModuleR.Controls
{
    public interface IOptionControl
    {
        event PropertyChangedEventHandler PropertyChanged;

        string GetRCode();
    }
}