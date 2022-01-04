using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Xml;

namespace SharpStatistics.EasyERAddIn.Functions.Controls
{
    /// <summary>
    /// Interaction logic for SharpComboBox.xaml
    /// </summary>
    public partial class SharpComboBox : UserControl,IFunctionControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string selectedOption;
        private string label;
        private string variableName;
        private List<string> options;
        private Dictionary<string, string> selectionLookup;
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
            get{return options;}
            set{options = value;
                OnPropertyChanged("Options");}
        }

        public string Label
        {
            get{return label;}
            set{label = value; }
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

        public SharpComboBox(string label, string variable,bool isProperty)
        {
            InitializeComponent();
            ControlPanel.DataContext = this;
            this.label = label;
            variableName = variable;
            options = new List<string>();
            //Options = options;
            //this.SelectedOption = options.FirstOrDefault();
            selectionLookup = new Dictionary<string, string>();
            property = isProperty;
        }

        public void SetValues(List<object> newValues)
        {
            options.Clear();
            selectionLookup.Clear();
            foreach (var newItem in newValues)
            {
                XmlNode[] things = (XmlNode[])newItem;
                string[] result = things[0].Value.Split(',');
                selectionLookup.Add(result[0], result[1]);
                options.Add(result[0]);
            }
            selectedOption = options[0];
        }

        public string GetRCode()
        {
            string rCode = "";
            if (property)
            {
                rCode = string.Format("\"{0}\"", selectionLookup[SelectedOption].ToString());
            }
            else
            {
                rCode = SelectedOption;
            }
            return rCode;
        }
    }
}
