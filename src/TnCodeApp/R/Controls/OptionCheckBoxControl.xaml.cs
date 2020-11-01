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
        private bool _selected;

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
            get { return _selected; }
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                    OnPropertyChanged("Selected");
                }
            }
        }

        public OptionCheckBoxControl(Property prop)
        {
            InitializeComponent();
            ControlPanel.DataContext = this;
            property = prop;
            _selected = bool.Parse(property.Value);
        }

        public string GetRCode()
        {
            string rCode = null;
            if (_selected)
            {
                rCode = property.Name + "=TRUE";
            }
            else
            {
                rCode = property.Name + "=FALSE";
            }
            return rCode;
        }
    }
}
