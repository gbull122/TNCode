using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Controls;
using TnCode.Core.R.Charts.Ggplot.Layer;

namespace TnCode.TnCodeApp.R.Controls
{
    /// <summary>
    /// Interaction logic for OptionPropertyControl.xaml
    /// </summary>
    public partial class OptionPropertyControl : UserControl, INotifyPropertyChanged, IOptionControl
    {
        public event PropertyChangedEventHandler PropertyChanged;
        //private string _propertyName;
        private List<string> options;
        private Dictionary<string, string> selectionLookup;
        private string selectedOption;
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

        public List<string> Options
        {
            get { return options; }
            set
            {
                options = value;
                OnPropertyChanged("Options");
            }
        }

        public string SelectedOption
        {
            get
            {
                return selectedOption;
            }
            set
            {
                if (selectedOption != value)
                {
                    selectedOption = value;
                    OnPropertyChanged("SelectedOption");
                }
            }
        }

        public OptionPropertyControl(Property prop)
        {
            InitializeComponent();
            ControlPanel.DataContext = this;
            property = prop;
            options = new List<string>();
            selectionLookup = new Dictionary<string, string>();
        }

        public void SetValues(List<Option> newValues)
        {
            options.Clear();
            selectionLookup.Clear();
            foreach (var newItem in newValues)
            {
                selectionLookup.Add(newItem.Name, newItem.Value);
                options.Add(newItem.Name);
            }
            selectedOption = options[0];
        }

    }
}
