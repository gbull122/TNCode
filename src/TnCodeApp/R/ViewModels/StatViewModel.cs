using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TnCode.Core.R.Charts.Ggplot;
using TnCode.Core.R.Charts.Ggplot.Layer;
using TnCode.TnCodeApp.R.Controls;

namespace TnCode.TnCodeApp.R.ViewModels
{
    public class StatViewModel:BindableBase
    {
        private ObservableCollection<IOptionControl> statControls;
        private readonly IXmlService xmlService;

        public List<string> Stats { get; }
        private Stat currentStat;
        private string currentStatName;

        private bool statSet;

        public event EventHandler StatChanged;

        public string CurrentStatName
        {
            get => currentStatName;
            set
            {
                currentStatName = value;
                SelectedStatChanged();
                RaisePropertyChanged(nameof(CurrentStatName));
            }
        }

        public ObservableCollection<IOptionControl> StatControls
        {
            get { return statControls; }
            set
            {
                statControls = value;
                RaisePropertyChanged(nameof(StatControls));
            }
        }

        public StatViewModel(IXmlService xService)
        {
            xmlService = xService;

            Stats = Enum.GetNames(typeof(Ggplot.Stats)).ToList();

            statControls = new ObservableCollection<IOptionControl>();

            currentStat = new Stat();
        }

        private void SelectedStatChanged()
        {
            if (statSet)
                return;
            
            var newStat = xmlService.LoadStat(currentStatName);
            currentStat = newStat;

            SetControls();
            
            statSet = false;
        }

        internal void SetStat(Stat statistic)
        {
            currentStat = statistic;

            statSet = true;
            CurrentStatName = statistic.Name;
            SetControls();
        }

        internal void SetControls()
        {
            foreach (var control in statControls)
            {
                control.PropertyChanged -= ControlChanged;
            }
            StatControls.Clear();

            foreach (var aProp in currentStat.Properties)
            {
                var oControl = new OptionPropertyControl(aProp.Tag, aProp.Name);
                oControl.SetValues(aProp.Options);
                oControl.PropertyChanged += ControlChanged;
                StatControls.Add(oControl);
            }

            foreach (var prop in currentStat.Booleans)
            {
                var control = new OptionCheckBoxControl(prop.Tag, prop.Name, bool.Parse(prop.Value));
                control.PropertyChanged += ControlChanged;
                StatControls.Add(control);
            }

            foreach (var prop in currentStat.Values)
            {
                double.TryParse(prop.Value, out double result);
                var control = new OptionValueControl(prop.Tag, prop.Name, result);
                control.PropertyChanged += ControlChanged;
                StatControls.Add(control);
            }

        }

        private  void ControlChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            StatChanged?.Invoke(this, new EventArgs());
        }

    }
}
