using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace SharpStatistics.EasyERAddIn.Functions.Controls
{
    /// <summary>
    /// Interaction logic for SharpComboVariable.xaml
    /// </summary>
    public partial class SharpComboVariable : UserControl,IFunctionControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string selectedOption;
        private string label;
        private string variableName;
        public List<string> options;
        private bool property;

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool ReplaceAll
        {
            get { return true; }
            set { ReplaceAll = value; }
        }

        public string VariableName
        {
            get { return variableName; }
            set { variableName = value; }
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

        public string Label
        {
            get { return label; }
            set { label = value; }
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

        public bool IsValid
        {
            get { return true; }
            set { IsValid = value; }
        }

        public SharpComboVariable(string label, string variable, bool isProperty)
        {
            InitializeComponent();
            ControlPanel.DataContext = this;
            this.label = label;
            variableName = variable;
            options = new List<string>();
            //Options = options;
            //this.SelectedOption = options.FirstOrDefault();
            //selectionLookup = new Dictionary<string, string>();
            property = isProperty;
        }

        public void SetValues(List<object> newValues)
        {
            vCombo.ItemsSource = null;
            vCombo.SelectedIndex =-1;
            vCombo.Items.Clear();
            
            options.Clear();
            
            //selectionLookup.Clear();
            foreach (var newItem in newValues)
            {
                //XmlNode[] things = (XmlNode[])newItem;
                //string result = ;
                //selectionLookup.Add(result[0], result[1]);
                options.Add(newItem.ToString());
            }
            vCombo.SelectedIndex = 0;
            vCombo.ItemsSource = options;
            selectedOption = options[0];
            
        }

        public string GetRCode()
        {
            string rCode = "";
            if (property)
            {
                rCode = string.Format("\"{0}\"", SelectedOption.ToString());
            }
            else
            {
                rCode = SelectedOption;
            }
            return rCode;
        }
    }
}
