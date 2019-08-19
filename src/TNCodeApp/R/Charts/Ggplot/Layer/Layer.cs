﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TNCodeApp.R.Charts.Ggplot.Layer
{
    public class Layer : ILayer, INotifyPropertyChanged
    {
        private string data;
        private string geom;
        private bool showLegend;
        private bool showInPlot;

        public event PropertyChangedEventHandler PropertyChanged;

        private List<Parameter> labels;

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        } 

        public string Data
        {
            get { return data; }
            set
            {
                data = value;
                OnPropertyChanged("Data");
            }
        }

        public string Geom
        {
            get { return geom; }
            set
            {
                geom = value;
                OnPropertyChanged("Geom");
            }
        }

        public Aesthetic Aes { get; set; }

        public string Stat
        {
            get; set;
        }

        public bool ShowLegend
        {
            get { return showLegend; }
            set
            {
                showLegend = value;
                OnPropertyChanged("ShowLegend");
            }
        }

        public bool ShowInPlot
        {
            get { return showInPlot; }
            set
            {
                showInPlot = value;
                OnPropertyChanged("ShowInPlot");
            }
        }

        public string Position
        {
            get; set;
        }

        public Layer(string geom)
        {
            Geom = geom;

            labels = new List<Parameter>();
        }

        public string Command()
        {
            var command = new StringBuilder();

            if (Aes == null || !IsValid())
                return string.Empty;

            command.Append("layer(");
            command.Append("data=" + Data + ",");
            command.Append("geom=\"" + Geom.ToString() + "\",");
            command.Append("mapping=" + Aes.Command() + ",");
            command.Append("stat=" + string.Format("\"{0}\"", Aes.DefaultStat.ToLower()) + ",");
            command.Append("position=" + string.Format("\"{0}\"", Aes.DefaultPosition.ToLower()) + ",");
            //command.Append(ParametersCommand() + ",");
            command.Append("show.legend=" + ShowLegend.ToString().ToUpper());
            command.Append(")");
            command.Append(DoLabels());
            //command.Append(DoScales());
            //command.Append(DoFacet());

            return command.ToString();
        }

        private string DoLabels()
        {
            List<string> titles = new List<string>();
            if (labels.Count == 0)
                return string.Empty;

            foreach (var l in labels)
            {
                if (!string.IsNullOrEmpty(l.Command()))
                    titles.Add(l.Command());
            }

            if (titles.Count == 0)
                return string.Empty;

            return "+labs(" + string.Join(",", titles) + ")";
        }

        private bool IsValid()
        {
            foreach(var val in Aes.AestheticValues)
            {
                if (val.Required)
                    if (string.IsNullOrEmpty(val.Entry))
                        return false;
            }

            return true;
        }
    }
}