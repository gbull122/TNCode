using Prism.Mvvm;
using System;
using System.Collections.Generic;
using TnCode.Core.R.Charts.Ggplot.Layer;

namespace TnCode.TnCodeApp.R.ViewModels
{
    public class FacetViewModel: BindableBase
    {
        private List<string> variables;
        public EventHandler FacetChanged;

        public Facet CurrentFacet { get; }

        public List<string> Variables
        {
            get => variables;
            set
            {
                variables = value;
                RaisePropertyChanged(nameof(Variables));
            }
        }

        public string XVariableFacet
        {
            get => CurrentFacet.XVariable;
            set
            {
                CurrentFacet.XVariable = value;
                RaisePropertyChanged(nameof(XVariableFacet));
            }
        }

        public string YVariableFacet
        {
            get => CurrentFacet.YVariable;
            set
            {
                CurrentFacet.YVariable = value;
                RaisePropertyChanged(nameof(YVariableFacet));
            }
        }

        public bool IsFreeSpace
        {
            get => CurrentFacet.IsFreeSpace;
            set
            {
                CurrentFacet.IsFreeSpace = value;
                RaisePropertyChanged(nameof(IsFreeSpace));
            }
        }

        public bool IsFreeScale
        {
            get => CurrentFacet.IsFreeScale;
            set
            {
                CurrentFacet.IsFreeScale = value;
                RaisePropertyChanged(nameof(IsFreeScale));
            }
        }

        public FacetViewModel()
        {
            CurrentFacet = new Facet();
        }

    }
}