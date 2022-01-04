using Prism.Commands;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using TnCode.Core.R.Charts.Ggplot.Layer;

namespace TnCode.TnCodeApp.R.Controls
{
    /// <summary>
    /// Interaction logic for OptionMultiControl.xaml
    /// </summary>
    public partial class OptionMultiControl : UserControl, INotifyPropertyChanged, IOptionControl
    {
        public event PropertyChangedEventHandler PropertyChanged;
        //private string _propertyName;
        private List<SelectableItem> options;
        private Dictionary<string, string> selectionLookup;
        private List<string> selectedOptions;
        private Property property;

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        public List<SelectableItem> Options
        {
            get { return options; }
            set
            {
                options = value;
                OnPropertyChanged("Options");
            }
        }

        public OptionMultiControl(Property prop)
        {
            InitializeComponent();
            ControlPanel.DataContext = this;
            property = prop;
            options = new List<SelectableItem>();
            selectionLookup = new Dictionary<string, string>();
        }

        public void SetValues(List<Option> newValues)
        {
            options.Clear();
            selectionLookup.Clear();
            foreach (var newItem in newValues)
            {
                selectionLookup.Add(newItem.Name, newItem.Value);
                options.Add(new SelectableItem(newItem.Name,newItem.Selected));
            }

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OnPropertyChanged();
        }
    }
}
