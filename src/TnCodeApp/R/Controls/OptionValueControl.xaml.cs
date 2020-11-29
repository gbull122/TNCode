using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using TnCode.Core.R.Charts.Ggplot.Layer;

namespace TnCode.TnCodeApp.R.Controls
{

    public partial class OptionValueControl : UserControl, INotifyPropertyChanged, IOptionControl
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Property property;

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string Label
        {
            get { return property.Tag; }
            set
            {
                property.Tag = value;
                OnPropertyChanged("Label");
            }
        }

        public string PropertyValue
        {
            get { return property.Value; }
            set
            {
                property.Value = value;
                OnPropertyChanged("PropertyValue");
            }
        }

        public OptionValueControl(Property prop)
        {
            InitializeComponent();
            ControlPanel.DataContext = this;
            property = prop;
        }

    }
}
