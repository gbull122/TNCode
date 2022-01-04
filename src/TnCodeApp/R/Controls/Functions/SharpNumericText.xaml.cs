using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

namespace SharpStatistics.EasyERAddIn.Functions.Controls
{
    /// <summary>
    /// Interaction logic for SharpNumericText.xaml
    /// </summary>
    public partial class SharpNumericText : UserControl, IFunctionControl, INotifyPropertyChanged
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

        public SharpNumericText(string label, string variable)
        {
            InitializeComponent();
            this.label = label;
            variableName = variable;
            ControlPanel.DataContext = this;
        }

        public string VariableName
        {
            get { return variableName; }
            set { variableName = value; }
        }

        public string Label
        {
            get { return label; }
            set { label = value; }
        }

        public bool ReplaceAll
        {
            get { return true; }
            set { ReplaceAll = value; }
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

        public string GetRCode()
        {
            string rcode = userText;
            return rcode;
        }

        public void SetValues(List<object> newValues)
        {
            XmlNode[] things = (XmlNode[])newValues[0];
            userText = (things[0].Value.ToString().Replace("\"", ""));
            if(userText.StartsWith("$"))
            {

            }
        }

        private void MyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UserText = MyTextBox.Text;
            }
        }
    }
}
