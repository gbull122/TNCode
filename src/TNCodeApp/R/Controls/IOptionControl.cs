using System.ComponentModel;

namespace TNCodeApp.R.Controls
{
    public interface IOptionControl
    {
        event PropertyChangedEventHandler PropertyChanged;

        string GetRCode();
    }
}