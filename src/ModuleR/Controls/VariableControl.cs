using ModuleR.Charts.Ggplot.Layer;
using System.Collections.Generic;
using System.ComponentModel;

namespace ModuleR.Controls
{
    public class VariableControl : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<string> variables;

        private AestheticValue aestheticValue;

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public VariableControl()
        {

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

        public string SelectedVariable
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
            get { return aestheticValue.IsFactor==null ? false: aestheticValue.IsFactor; }
            set
            {
                aestheticValue.IsFactor = value;
                OnPropertyChanged("Factor");
            }
        }

        public VariableControl(AestheticValue aesValue, List<string> variableNames)
        {
            aestheticValue = aesValue;
            variables = variableNames;
        }
    }
}

