using System.ComponentModel;
using System.Windows.Controls;
using TnCode.Core.R.Charts.Ggplot.Layer;

namespace TnCode.TnCodeApp.R.Controls
{
    /// <summary>
    /// Interaction logic for OptionCheckBoxControl.xaml
    /// </summary>
    public partial class OptionCheckBoxControl : UserControl, INotifyPropertyChanged, IOptionControl
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

        public bool Selected
        {
            get { return bool.Parse(property.Value); }
            set
            {
                property.Value = value.ToString();
                OnPropertyChanged("Selected");
            }
        }

        public OptionCheckBoxControl(Property prop)
        {
            InitializeComponent();
            ControlPanel.DataContext = this;
            property = prop;
        }
    }
}
