using Prism.Mvvm;
using System.Collections.Generic;
using TNCodeApp.R.Charts.Ggplot.Layer;

namespace TNCodeApp.R.ViewModels
{
    public class GgplotTitleViewModel:BindableBase
    {
        private Parameter xAxisTitle;
        private Parameter yAxisTitle;
        private Parameter mainTitle;
        private Parameter subtitle;
        private Parameter caption;
        private Parameter legend;
        private List<Parameter> labels;
        public string MainTitle
        {
            get { return mainTitle.Value; }
            set
            {
                mainTitle.Value = value;
                RaisePropertyChanged("MainTitle");
            }
        }

        public string XAxisTitle
        {
            get { return xAxisTitle.Value; }
            set
            {
                xAxisTitle.Value = value;
                RaisePropertyChanged("XAxisTitle");
            }
        }

        public string YAxisTitle
        {
            get { return yAxisTitle.Value; }
            set
            {
                yAxisTitle.Value = value;
                RaisePropertyChanged("YAxisTitle");
            }
        }

        public string Subtitle
        {
            get { return subtitle.Value; }
            set
            {
                subtitle.Value = value;
                RaisePropertyChanged("SubTitle");
            }
        }

        public string Caption
        {
            get { return caption.Value; }
            set
            {
                caption.Value = value;
                RaisePropertyChanged("Caption");
            }
        }

        public string Legend
        {
            get { return legend.Value; }
            set
            {
                legend.Value = value;
                RaisePropertyChanged("Legend");
            }
        }

        public GgplotTitleViewModel()
        {
            labels = new List<Parameter>();

            mainTitle = new Parameter("title", "");
            mainTitle.UseQuotes = true;
            labels.Add(mainTitle);

            xAxisTitle = new Parameter("x", "");
            xAxisTitle.UseQuotes = true;
            labels.Add(xAxisTitle);

            yAxisTitle = new Parameter("y", "");
            yAxisTitle.UseQuotes = true;
            labels.Add(yAxisTitle);

            subtitle = new Parameter("subtitle", "");
            subtitle.UseQuotes = true;
            labels.Add(subtitle);

            caption = new Parameter("caption", "");
            caption.UseQuotes = true;
            labels.Add(caption);

            legend = new Parameter("legend", "");
            legend.UseQuotes = true;
            labels.Add(legend);
        }

        public string Command()
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
    }
}
