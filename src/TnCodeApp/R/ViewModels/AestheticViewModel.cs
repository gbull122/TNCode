using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TnCode.Core.R.Charts.Ggplot.Layer;
using TnCode.TnCodeApp.R.Controls;

namespace TnCode.TnCodeApp.R.ViewModels
{
    public class AestheticViewModel : BindableBase
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IXmlService xmlService;

        private ObservableCollection<VariableControl> aesControls;

        private List<string> variables;

        private Aesthetic currentAesthetic;

        public event EventHandler AesChanged;

        public List<string> Variables
        {
            get => variables;
            set
            {
                variables = value;
                RaisePropertyChanged(nameof(Variables));
            }
        }

        public ObservableCollection<VariableControl> AesControls
        {
            get { return aesControls; }
            set
            {
                aesControls = value;
                RaisePropertyChanged(nameof(AesControls));
            }
        }

        public AestheticViewModel(IXmlService xService, IEventAggregator eventAggr)
        {
            xmlService = xService;
            eventAggregator = eventAggr;

            aesControls = new ObservableCollection<VariableControl>();
            variables = new List<string>();
        }

        public void SetAesthetic(Aesthetic aesthetic)
        {
            if (currentAesthetic == null)
            {
                currentAesthetic = aesthetic;
            }
            else
            {
                var mergedAesthetic = new Aesthetic();

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

            SetControls();

            //TODO property controls
        }


        public void SetControls()
        {
            foreach (var variableControl in aesControls)
            {
                variableControl.PropertyChanged -= VariableControl_PropertyChanged;
            }

            AesControls.Clear();

            foreach (var aValue in currentAesthetic.AestheticValues)
            {
                var variableControl = new VariableControl(eventAggregator, aValue, variables);
                variableControl.PropertyChanged += VariableControl_PropertyChanged;
                AesControls.Add(variableControl);
            }

            //AesControls = newControls;
        }

        private void VariableControl_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            AesChanged?.Invoke(this, new EventArgs());
        }

        internal void UpdateVariables(List<string> lists)
        {
            Variables = lists;
            SetControls();
        }
    }
}
