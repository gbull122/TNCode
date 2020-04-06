using Prism.Mvvm;
using System.Collections.Generic;

namespace TNCodeApp.R.ViewModels
{
    public class GgplotFacetViewModel:BindableBase
    {
        private bool isFreeScale;
        private bool isFreeSpace;
        private List<string> variables;

        public bool IsFreeScale
        {
            get { return isFreeScale; }
            set
            {
                isFreeScale = value;
                RaisePropertyChanged(nameof(IsFreeScale));
            }
        }

        public bool IsFreeSpace
        {
            get { return isFreeSpace; }
            set
            {
                isFreeSpace = value;
                RaisePropertyChanged(nameof(IsFreeSpace));
            }
        }

        public List<string> Variables
        {
            get { return variables; }
            set
            {
                variables = value;
                RaisePropertyChanged(nameof(Variables));
            }
        }

        public GgplotFacetViewModel()
        {

        }
    }
}
