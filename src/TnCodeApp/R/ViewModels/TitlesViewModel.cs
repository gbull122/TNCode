using Prism.Mvvm;
using System.Collections.Generic;
using System.Reflection.Metadata;

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

        private List<Parameter> TitleParameters { get; }

        public TitlesViewModel()
        {
            TitleParameters = new List<Parameter>();
        }

    }
}
