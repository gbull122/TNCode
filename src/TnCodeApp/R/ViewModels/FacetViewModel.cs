using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace TnCode.TnCodeApp.R.ViewModels
{
    public class FacetViewModel: BindableBase
    {

        private string xVariableFacet;

        public string XVariableFacet
        {
            get => xVariableFacet;
            set
            {
                xVariableFacet = value;
                RaisePropertyChanged(nameof(XVariableFacet));
            }
        }

        private string yVariableFacet;

        public string YVariableFacet
        {
            get => yVariableFacet;
            set
            {
                yVariableFacet = value;
                RaisePropertyChanged(nameof(YVariableFacet));
            }
        }
    }
}
