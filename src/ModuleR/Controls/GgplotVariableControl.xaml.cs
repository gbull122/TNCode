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
        private bool factor;

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Name
        {
            get { return Name; }
            set { Name = value; }
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

        public bool Factor
        {
            get { return factor; }
            set { factor=value; }
        }

        public GgplotVariableControl(AestheticValue aestheticValue, List<string> variableNames)
        {
            InitializeComponent();
            Variables = variableNames;
        }


    }
}
