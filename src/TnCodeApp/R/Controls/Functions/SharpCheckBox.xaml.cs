using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Xml;

namespace SharpStatistics.EasyERAddIn.Functions.Controls
{
    /// <summary>
    /// Interaction logic for SharpCheckBox.xaml
    /// </summary>
    public partial class SharpCheckBox : UserControl, IFunctionControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool selected;
        private string variableName;
        private string label;


        #region Properties

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

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //public void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        public string VariableName
        {
            get { return variableName; }
            set { variableName = value; }
        }

        public string Label
        {
            get { return label; }
            set { label= value; }
        }

        public bool Selected
        {
            get { return selected; }
            set
            {
                if (selected != value)
                {
                    selected = value;
                    OnPropertyChanged("Selected");
                }
            }
        }

        

        #endregion

        public SharpCheckBox(string label, string variable, bool selected)
        {
            InitializeComponent();
            ControlPanel.DataContext = this;
            this.label = label;
            this.variableName = variable;
            this.selected = selected;
        }

        #region Public Methods

        public void SetValues(List<object> newValues)
        {
            XmlNode[] things = (XmlNode[])newValues[0];
            selected = Convert.ToBoolean(Convert.ToBoolean(things[0].Value));
        }

        public string GetRCode()
        {
            string rCode = null;
            if (selected)
            {
                rCode = "TRUE";
            }
            else
            {
                rCode = "FALSE";
            }
            return rCode;
        }

        #endregion

    }
}
