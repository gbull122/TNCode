using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

namespace SharpStatistics.EasyERAddIn.Functions.Controls
{
    /// <summary>
    /// Interaction logic for SharpTextBox.xaml
    /// </summary>
    public partial class SharpTextBox : UserControl,IFunctionControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string userText;
        private string label;
        private string variableName;

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

        public string Label
        {
            get{return label;}
            set {label = value; }
        }

        public string UserText
        {
            get { return userText; }
            set
            {
                if (userText != value)
                {
                    userText = value;
                    OnPropertyChanged("UserText");
                }
            }
        }

        public bool IsValid
        {
            get { return true; }
            set { IsValid = value; }
        }

        public SharpTextBox(string label,string variable, string startText)
        {
            InitializeComponent();
            this.label = label;
            variableName = variable;
            userText = startText;
            ControlPanel.DataContext = this;
        }

        public string GetRCode()
        {
            string rcode = string.Format("\"{0}\"", userText);
            return rcode;
        }

        public void SetValues(List<object> newValues)
        {
            XmlNode[] things = (XmlNode[])newValues[0];
            userText = (things[0].Value.ToString().Replace("\"", ""));
        }

        private void MyTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UserText = MyTextBox.Text;
            }
        }
    }
}
