using Prism.Mvvm;
using System.Collections.Generic;

namespace TnCode.TnCodeApp.R.ViewModels
{
    public class TitlesViewModel: BindableBase
    {
        private string xAxisTitle;
        private string yAxisTitle;
        private string mainTitle;
        private string subTitle;
        private string caption;

        public string XAxisTitle
        {
            get => xAxisTitle;
            set
            {
                xAxisTitle = value;
                RaisePropertyChanged(nameof(XAxisTitle));
            }
        }

        public string YAxisTitle
        {
            get => yAxisTitle;
            set
            {
                yAxisTitle = value;
                RaisePropertyChanged(nameof(YAxisTitle));
            }
        }

        public string MainTitle
        {
            get => mainTitle;
            set
            {
                mainTitle = value;
                RaisePropertyChanged(nameof(MainTitle));
            }
        }

        public string SubTitle
        {
            get => subTitle;
            set
            {
                subTitle = value;
                RaisePropertyChanged(nameof(SubTitle));
            }
        }

        public string Caption
        {
            get => caption;
            set
            {
                caption = value;
                RaisePropertyChanged(nameof(Caption));
            }
        }

        private List<TnCode.Core.R.Charts.Ggplot.Layer.Parameter> titleParameters { get; }

        public TitlesViewModel()
        {
            titleParameters = new List<Core.R.Charts.Ggplot.Layer.Parameter>();
        }

        public List<Core.R.Charts.Ggplot.Layer.Parameter> GetParameters()
        {
            titleParameters.Clear();
            if (!string.IsNullOrEmpty(xAxisTitle))
            {
                Core.R.Charts.Ggplot.Layer.Parameter param = new Core.R.Charts.Ggplot.Layer.Parameter("x", xAxisTitle,false,true);
                titleParameters.Add(param);
            }

            if (!string.IsNullOrEmpty(yAxisTitle))
            {
                Core.R.Charts.Ggplot.Layer.Parameter param = new Core.R.Charts.Ggplot.Layer.Parameter("y", yAxisTitle, false, true);
                titleParameters.Add(param);
            }

            if (!string.IsNullOrEmpty(mainTitle))
            {
                Core.R.Charts.Ggplot.Layer.Parameter param = new Core.R.Charts.Ggplot.Layer.Parameter("title", mainTitle, false, true);
                titleParameters.Add(param);
            }

            if (!string.IsNullOrEmpty(subTitle))
            {
                Core.R.Charts.Ggplot.Layer.Parameter param = new Core.R.Charts.Ggplot.Layer.Parameter("subtitle", subTitle, false, true);
                titleParameters.Add(param);
            }

            if (!string.IsNullOrEmpty(caption))
            {
                Core.R.Charts.Ggplot.Layer.Parameter param = new Core.R.Charts.Ggplot.Layer.Parameter("caption", caption, false, true);
                titleParameters.Add(param);
            }
            return titleParameters;
        }
    }
}
