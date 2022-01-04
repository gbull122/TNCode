using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SharpStatistics.EasyERAddIn.Functions.Controls
{
    /// <summary>
    /// Interaction logic for SharpMultiVariable.xaml
    /// </summary>
    public partial class SharpMultiVariableList : UserControl, IFunctionControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string label;
        private string variableName;
        private string dataName = "raddinData";
        public List<VariableCheckedListItem> options;
        private bool property;

        protected void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string VariableName
        {
            get { return variableName; }
            set { variableName = value; }
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

        public string Label
        {
            get { return label; }
            set { label = value; }
        }

        public bool IsValid
        {
            get { return true; }
            set { IsValid = value; }
        }

        public bool ReplaceAll
        {
            get { return false; }
            set { ReplaceAll = value; }
        }

        public SharpMultiVariableList(string label, string variable, bool isProperty)
        {
            InitializeComponent();
            ControlPanel.DataContext = this;
            this.label = label;
            variableName = variable;
            options = new List<VariableCheckedListItem>();
            property = isProperty;
        }

        public void SetValues(List<object> newValues)
        {
            options.Clear();
            foreach (var newItem in newValues)
            {
                options.Add(new VariableCheckedListItem(newItem.ToString(), true));
            }
        }

        public string GetRCode()
        {
            StringBuilder rCode = new StringBuilder();
            StringBuilder names = new StringBuilder();

            var selected = from op in options
                           where op.IsChecked
                           select op.Name;

            var quotedOptions = string.Join(",", selected.Select(x=>"\""+x+"\""));
            var prefixedOption = string.Join(",", selected.Select(x => dataName + "$" + x));

            rCode.Append(VariableName + "<-list(" + prefixedOption+"),");
            names.Append("names=c(" + quotedOptions+")");

            return rCode.AppendLine(names.ToString()).ToString();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            OnPropertyChanged("Options");
        }
    }
}
