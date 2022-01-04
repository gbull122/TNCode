using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Xml;

namespace SharpStatistics.EasyERAddIn.Functions.Controls
{
    /// <summary>
    /// Interaction logic for SharpSlider.xaml
    /// </summary>
    public partial class SharpSlider : UserControl,IFunctionControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string variableName;
        private string label;
        private double sliderValue;
        private int numberOfDecimals;

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

        public string SliderLabel
        {
            get { return label; }
            set { label = value;
                OnPropertyChanged("Label");
            }
        }

        public string VariableName
        {
            get { return variableName;}
            set {variableName=value;
                OnPropertyChanged("VariableName");
            }
        }

        public double SliderValue
        {
            get { return sliderValue; }
            set
            {
                if (sliderValue != value)
                {
                    sliderValue = value;
                    OnPropertyChanged("Selected");
                }
            }
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

        public SharpSlider(string label, string variable, List<object> values)
        {
            InitializeComponent();
            ControlPanel.DataContext = this;
            SliderLabel = label;
            VariableName = variable;
            SetValues(values);
        } 

        public string GetRCode()
        {
            string rcode = Math.Round(SliderValue,2).ToString();
            return rcode;
        }

        public void SetValues(List<object> newValues)
        {
            //min,max,interval,factor,start
            XmlNode[] min = (XmlNode[])newValues[0];
            SliderControl.Minimum = double.Parse((min[0].Value.ToString().Replace("\"", "")));
            XmlNode[] max = (XmlNode[])newValues[1];
            SliderControl.Maximum = double.Parse((max[0].Value.ToString().Replace("\"", "")));
            XmlNode[] interval = (XmlNode[])newValues[2];
            SliderControl.TickFrequency = double.Parse((interval[0].Value.ToString().Replace("\"", "")));
            SliderControl.SmallChange = double.Parse((interval[0].Value.ToString().Replace("\"", "")));
            numberOfDecimals = NumberOfDecimalPlaces(SliderControl.SmallChange);
            XmlNode[] start = (XmlNode[])newValues[3];
            SliderValue = double.Parse((start[0].Value.ToString().Replace("\"", "")));
        }

        private int NumberOfDecimalPlaces(double number)
        {
            bool start = false;
            int count = 0;
            foreach (var s in number.ToString())
            {
                if (s == '.')
                {
                    start = true;
                }
                else if (start)
                {
                    count++;
                }
            }

            return count;

        }
    }
}
