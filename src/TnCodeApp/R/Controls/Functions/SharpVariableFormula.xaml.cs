using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SharpStatistics.EasyERAddIn.Functions.Controls
{
    /// <summary>
    /// Interaction logic for SharpVariableFormula.xaml
    /// </summary>
    public partial class SharpVariableFormula : UserControl, IFunctionControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string label;
        private string variableName;
        private string dataName = "raddinData";
        public List<VariableCheckedListItem> options;
        private bool property;
        public List<string> variables;
        private string selectedOption;
        private bool includeInteractions;

        protected void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool IncludeInteractions
        {
            get { return includeInteractions; }
            set
            {
                includeInteractions = value;
                OnPropertyChanged("IncludeInteractions");
            }
        }

        public List<VariableCheckedListItem> Options
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

        public List<string> Variables
        {
            get { return variables; }
            set
            {
                variables = value;
                OnPropertyChanged("Options");
            }
        }

        public string Label
        {
            get { return label; }
            set { label = value; }
        }

        public string VariableName
        {
            get { return variableName; }
            set { variableName = value; }
        }

        public bool IsValid
        {
            get { return true; }
            set { IsValid = value; }
        }

        public bool ReplaceAll
        {
            get { return true; }
            set { ReplaceAll = value; }
        }

        public SharpVariableFormula(string label, string variable, bool isProperty)
        {
            InitializeComponent();
            ControlPanel.DataContext = this;
            this.label = label;
            variableName = variable;
            options = new List<VariableCheckedListItem>();
            variables = new List<string>();
            property = isProperty;
        }

        public string GetRCode()
        {
            StringBuilder rCode = new StringBuilder();

            var selected = from op in options
                           where op.IsChecked
                           select op.Name;

            string addedOptions = null;

            if (IncludeInteractions)
            {
                addedOptions = string.Join("*", selected);
            }
            else
            {
                addedOptions = string.Join("+", selected);
            }
            rCode.Append(addedOptions);

            rCode.Insert(0, selectedOption + "~");

            return rCode.ToString();
        }

        public void SetValues(List<object> newValues)
        {
            options.Clear();
            variables.Clear();
            foreach (var newItem in newValues)
            {
                options.Add(new VariableCheckedListItem(newItem.ToString(), false));
                variables.Add(newItem.ToString());
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            OnPropertyChanged("Options");
        }
    }
}
