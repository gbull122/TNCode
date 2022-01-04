using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Xml;

namespace SharpStatistics.EasyERAddIn.Functions.Controls
{
    /// <summary>
    /// Interaction logic for SharpColourControl.xaml
    /// </summary>
    public partial class SharpColourControl : UserControl,IFunctionControl, INotifyPropertyChanged
    {
        private List<string> colours = new List<string>() {"white","aliceblue","antiquewhite","aquamarine","azure","beige","bisque","black",
            "blanchedalmond","blue","blueviolet","brown","burlywood","cadetblue","chartreuse","chocolate","coral","cornflowerblue","cornsilk",
            "cyan","darkblue","darkcyan","darkgoldenrod","darkgray","darkgreen","darkgrey","darkkhaki","darkmagenta","darkolivegreen","darkorange",
            "darkorchid","darkred","darksalmon","darkseagreen","darkslateblue","darkslategray","darkslategrey","darkturquoise","darkviolet","deeppink",
            "deepskyblue","dimgray","dimgrey","dodgerblue","firebrick","floralwhite","forestgreen","gainsboro","ghostwhite","gold","goldenrod","gray",
            "green","greenyellow","grey","honeydew","hotpink","indianred","ivory","khaki","lavender","lavenderblush","lawngreen","lemonchiffon",
            "lightblue","lightcoral","lightcyan","lightgoldenrod","lightgoldenrodyellow","lightgray","lightgreen","lightgrey","lightpink","lightsalmon",
            "lightseagreen","lightskyblue","lightslateblue","lightslategray","lightslategrey","lightsteelblue","lightyellow","limegreen","linen","magenta",
            "maroon","mediumaquamarine","mediumblue","mediumorchid","mediumpurple","mediumseagreen","mediumslateblue","mediumspringgreen","mediumturquoise",
            "mediumvioletred","midnightblue","mintcream","mistyrose","moccasin","navajowhite","navy","navyblue","oldlace","olivedrab","orange","orangered",
            "orchid","palegoldenrod","palegreen","paleturquoise","palevioletred","papayawhip","peachpuff","peru","pink","plum","powderblue","purple","red",
            "rosybrown","royalblue","saddlebrown","salmon","sandybrown","seagreen","seashell","sienna","skyblue","slateblue","slategray","slategrey","snow",
            "springgreen","steelblue","tan","thistle","tomato","turquoise","violet","violetred","wheat","whitesmoke","yellow","yellowgreen"};

        public event PropertyChangedEventHandler PropertyChanged;
        private string selectedOption;
        private string label;
        private string variableName;

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
        public SharpColourControl(string label, string variable, List<object> startColour)
        {
            InitializeComponent();
            MainGrid.DataContext = this;
            this.label = label;
            variableName = variable;
            XmlNode[] tempColour = (XmlNode[])startColour[0];
            string temp = tempColour[0].Value.Replace("\"", "");
            if (colours.Contains(temp))
            {
                selectedOption = temp;
            }
            else
            {
                selectedOption = "black";
            }
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

        public List<string> Colours
        {
            get { return colours; }
            set
            {
                colours = value;
            }
        }

        public string GetRCode()
        {
            return string.Format("\"{0}\"", SelectedOption);
        }

        public void SetValues(List<object> newValues)
        {
            //Do nothing.
        }
    }
}
