using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace ModuleR.Controls
{

    public partial class OptionValueControl : UserControl, INotifyPropertyChanged, IOptionControl
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private double _userText;
        private string _label;

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string VariableName { get; set; }

        public string Label
        {
            get { return _label; }
            set
            {
                _label = value;
                OnPropertyChanged("Label");
            }
        }

        public double UserText
        {
            get { return _userText; }
            set
            {
                if (_userText != value)
                {
                    _userText = value;
                    OnPropertyChanged("UserText");
                }
            }
        }

        public OptionValueControl(string label, string variable, double defaultValue)
        {
            InitializeComponent();
            ControlPanel.DataContext = this;
            Label = label;
            VariableName = variable;
            UserText = defaultValue;
        }

        public string GetRCode()
        {
            return VariableName + "=" + _userText.ToString();
        }

        private void MyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (IsTextAllowed(MyTextBox.Text))
                    UserText = double.Parse(MyTextBox.Text);
            }
        }

        private void MyTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }
    }
}
