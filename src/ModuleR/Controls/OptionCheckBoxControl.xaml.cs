using System.ComponentModel;
using System.Windows.Controls;

namespace ModuleR.Controls
{
    /// <summary>
    /// Interaction logic for OptionCheckBoxControl.xaml
    /// </summary>
    public partial class OptionCheckBoxControl : UserControl, INotifyPropertyChanged, IOptionControl
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _selected;

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string VariableName { get; set; }

        public string Label { get; set; }

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

        public OptionCheckBoxControl(string label, string variable, bool selected)
        {
            InitializeComponent();
            ControlPanel.DataContext = this;
            Label = label;
            VariableName = variable;
            _selected = selected;
        }

        public string GetRCode()
        {
            string rCode = null;
            if (_selected)
            {
                rCode = VariableName + "=TRUE";
            }
            else
            {
                rCode = VariableName + "=FALSE";
            }
            return rCode;
        }
    }
}
