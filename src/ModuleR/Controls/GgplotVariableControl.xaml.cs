using ModuleR.Charts.Ggplot.Layer;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace ModuleR.Controls
{
    /// <summary>
    /// Interaction logic for GgplotVariableControl.xaml
    /// </summary>
    public partial class GgplotVariableControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<string> variables;

        private AestheticValue aestheticValue;

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public GgplotVariableControl()
        {
            DataContext = this;
        }

        public string PropertyName
        {
            get { return aestheticValue.Name; }
            set
            {
                aestheticValue.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public List<string> Variables
        {
            get
            {
                return variables;
            }
            set
            {
                variables = value;
                OnPropertyChanged("Variables");
            }
        }

        private string SelectedVariable
        {
            get { return aestheticValue.Entry; }
            set
            {
                aestheticValue.Entry = value;
                OnPropertyChanged("SelectedVariable");
            }
        }

        public bool? Factor
        {
            get { return aestheticValue.IsFactor; }
            set
            {
                aestheticValue.IsFactor = value;
                OnPropertyChanged("Factor");
            }
        }

        public GgplotVariableControl(AestheticValue aesValue, List<string> variableNames)
        {
            aestheticValue = aesValue;
            variables = variableNames;
            InitializeComponent();
        }

    }
}
