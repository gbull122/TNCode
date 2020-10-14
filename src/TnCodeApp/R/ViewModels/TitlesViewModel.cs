using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace TnCode.TnCodeApp.R.ViewModels
{
    public class TitlesViewModel: BindableBase
    {

        private List<Parameter> titleParameters;


        public TitlesViewModel()
        {
            titleParameters = new List<Parameter>();
        }

    }
}
