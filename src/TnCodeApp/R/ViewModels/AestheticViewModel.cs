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
        private ObservableCollection<IOptionControl> optionControls;
        private ObservableCollection<VariableControl> aesControls;

        private IList<string> variables;

        private Aesthetic currentAesthetic;

        public event EventHandler AesChanged;

        public IList<string> Variables
        {
            get => variables;
            set
            {
                variables = value;
                RaisePropertyChanged(nameof(Variables));
            }
        }
        public ObservableCollection<IOptionControl> OptionControls
        {
            get { return optionControls; }
            set
            {
                optionControls = value;
                RaisePropertyChanged(nameof(OptionControls));
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
            optionControls = new ObservableCollection<IOptionControl>();
            variables = new List<string>();
        }

        public void SetAesthetic(Aesthetic aesthetic)
        {
            currentAesthetic = aesthetic;

            SetControls();

            SetOptionControls();
        }

        public void MergeAesthetic(Aesthetic aesthetic)
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

                mergedAesthetic.DefaultPosition = aesthetic.DefaultPosition;
                mergedAesthetic.DefaultStat = aesthetic.DefaultStat;
                mergedAesthetic.Properties = aesthetic.Properties;
                mergedAesthetic.Booleans = aesthetic.Booleans;
                mergedAesthetic.Values = aesthetic.Values;

                currentAesthetic = mergedAesthetic;
            }
            SetControls();

            SetOptionControls();
        }

        public void SetControls()
        {
            if (currentAesthetic == null)
                return;

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
        }

        public void SetOptionControls()
        {
            foreach (var control in optionControls)
            {
                control.PropertyChanged -= VariableControl_PropertyChanged;
            }
            OptionControls.Clear();

            foreach (var aProp in currentAesthetic.Properties)
            {
                var oControl = new OptionPropertyControl(aProp);
                oControl.SetValues(aProp.Options);
                oControl.PropertyChanged += VariableControl_PropertyChanged;
                OptionControls.Add(oControl);
            }

            foreach (var prop in currentAesthetic.Booleans)
            {
                var control = new OptionCheckBoxControl(prop);
                control.PropertyChanged += VariableControl_PropertyChanged;
                OptionControls.Add(control);
            }

            foreach (var prop in currentAesthetic.Values)
            {
                double.TryParse(prop.Value, out double result);
                var control = new OptionValueControl(prop);
                control.PropertyChanged += VariableControl_PropertyChanged;
                OptionControls.Add(control);
            }
        }

        private void VariableControl_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            AesChanged?.Invoke(this, new EventArgs());
        }

        internal void UpdateVariables(IList<string> lists)
        {
            Variables = lists;
            SetControls();
        }
    }
}
