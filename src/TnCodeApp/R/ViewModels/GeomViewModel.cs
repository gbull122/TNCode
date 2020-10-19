using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TnCode.Core.R.Charts.Ggplot.Layer;
using TnCode.TnCodeApp.R.Controls;

namespace TnCode.TnCodeApp.R.ViewModels
{
    public class GeomViewModel: BindableBase
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IXmlService xmlService;

        private ObservableCollection<VariableControl> geomControls;

        private List<string> variables;

        private Aesthetic currentAesthetic;

        public event EventHandler GeomChanged;

        public List<string> Variables
        {
            get => variables;
            set
            {
                variables = value;
                RaisePropertyChanged(nameof(Variables));
            }
        }

        public ObservableCollection<VariableControl> GeomControls
        {
            get { return geomControls; }
            set
            {
                geomControls = value;
                RaisePropertyChanged(nameof(GeomControls));
            }
        }

        public GeomViewModel(IXmlService xService, IEventAggregator eventAggr)
        {
            xmlService = xService;
            eventAggregator = eventAggr;

            geomControls = new ObservableCollection<VariableControl>();
            variables = new List<string>();
        }

        public void SetAesthetics(Aesthetic aesthetic)
        {
            var mergedAesthetic = new Aesthetic();

            if (currentAesthetic == null)
            {
                currentAesthetic = aesthetic;
                return;
            }

            mergedAesthetic.DefaultStat = aesthetic.DefaultStat;
            mergedAesthetic.DefaultPosition = aesthetic.DefaultPosition;

            foreach (var aesValue in aesthetic.AestheticValues)
            {
                if (currentAesthetic.DoesAestheticContainValue(aesValue.Name))
                {
                    var existingValue = currentAesthetic.GetAestheticValueByName(aesValue.Name);
                    aesValue.Entry = existingValue.Entry;
                }
                mergedAesthetic.AestheticValues.Add(aesValue);
            }
            currentAesthetic = mergedAesthetic;
        }


        public void SetControls()
        {
            var newControls = new ObservableCollection<VariableControl>();
            foreach (var variableControl in geomControls)
            {
                variableControl.PropertyChanged -= VariableControl_PropertyChanged;
            }

            foreach (var aValue in currentAesthetic.AestheticValues)
            {
                var variableControl = new VariableControl(eventAggregator, aValue, variables);
                variableControl.PropertyChanged += VariableControl_PropertyChanged;
                newControls.Add(variableControl);
            }

            GeomControls = newControls;
        }

        private void VariableControl_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            GeomChanged?.Invoke(this,new EventArgs());
        }
    }
}
