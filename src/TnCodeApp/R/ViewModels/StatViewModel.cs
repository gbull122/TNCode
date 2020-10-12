using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TnCode.Core.R.Charts.Ggplot;
using TnCode.Core.R.Charts.Ggplot.Layer;
using TnCode.Core.Utilities;
using TnCode.TnCodeApp.R.Controls;

namespace TnCode.TnCodeApp.R.ViewModels
{
    public class StatViewModel:BindableBase
    {
        public DelegateCommand<string> SelectedStatChangedCommand { get; private set; }
        private ObservableCollection<IOptionControl> statControls;
        private readonly IXmlConverter xmlConverter;

        public List<string> Stats { get; }
        private string selectedStat;

        public string SelectedStat
        {
            get => selectedStat;
            set
            {
                selectedStat = value;
                RaisePropertyChanged(nameof(SelectedStat));
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

        public StatViewModel(IXmlConverter converter)
        {
            xmlConverter = converter;

            Stats = Enum.GetNames(typeof(Ggplot.Stats)).ToList();

            statControls = new ObservableCollection<IOptionControl>();

            SelectedStatChangedCommand = new DelegateCommand<string>(StatChanged);
        }

        public void StatChanged(string stat)
        {
            var statistic = LoadStat(stat);
            UpdateStat(statistic);
        }

        public async void StatChanged(Stat stat)
        {
            UpdateStat(stat);
        }

        private void UpdateStat(Stat stat)
        {
            var newControls = new ObservableCollection<IOptionControl>();

            foreach (var control in statControls)
            {
                control.PropertyChanged -= GControl_PropertyChanged;
            }
            statControls.Clear();

            foreach (var aProp in stat.Properties)
            {
                var oControl = new OptionPropertyControl(aProp.Tag, aProp.Name);
                oControl.SetValues(aProp.Options);
                oControl.PropertyChanged += GControl_PropertyChanged;
                newControls.Add(oControl);
            }

            foreach (var prop in stat.Booleans)
            {
                var control = new OptionCheckBoxControl(prop.Tag, prop.Name, bool.Parse(prop.Value));
                control.PropertyChanged += GControl_PropertyChanged;
                newControls.Add(control);
            }

            foreach (var prop in stat.Values)
            {
                double.TryParse(prop.Value, out double result);
                var control = new OptionValueControl(prop.Tag, prop.Name, result);
                control.PropertyChanged += GControl_PropertyChanged;
                newControls.Add(control);
            }
            StatControls = newControls;
        }

        private Stat LoadStat(string stat)
        {
            var statXml = Properties.Resources.ResourceManager.GetObject("stat_" + stat.ToLower());
            var statistic = xmlConverter.ToObject<Stat>(statXml.ToString());
            return statistic;
        }

        private async void GControl_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }
    }
}
