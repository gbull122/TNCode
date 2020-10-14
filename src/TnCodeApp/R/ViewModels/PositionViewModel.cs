using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using TnCode.Core.R.Charts.Ggplot.Layer;
using TnCode.TnCodeApp.R.Controls;

namespace TnCode.TnCodeApp.R.ViewModels
{
    public class PositionViewModel:BindableBase
    {
        private ObservableCollection<IOptionControl> positionControls;

        public ObservableCollection<IOptionControl> PositionControls
        {
            get { return positionControls; }
            set
            {
                positionControls = value;
                RaisePropertyChanged(nameof(PositionControls));
            }
        }

        public PositionViewModel()
        {
            positionControls = new ObservableCollection<IOptionControl>();


        }

        private void UpdatePosition(Position pos)
        {
            var newControls = new ObservableCollection<IOptionControl>();

            foreach (var control in positionControls)
            {
                control.PropertyChanged -= GControl_PropertyChanged;
            }
            positionControls.Clear();

            foreach (var aProp in pos.Properties)
            {
                var oControl = new OptionPropertyControl(aProp.Tag, aProp.Name);
                oControl.SetValues(aProp.Options);
                oControl.PropertyChanged += GControl_PropertyChanged;
                newControls.Add(oControl);
            }

            foreach (var prop in pos.Booleans)
            {
                var control = new OptionCheckBoxControl(prop.Tag, prop.Name, bool.Parse(prop.Value));
                control.PropertyChanged += GControl_PropertyChanged;
                newControls.Add(control);
            }

            foreach (var prop in pos.Values)
            {
                double.TryParse(prop.Value, out double result);
                var control = new OptionValueControl(prop.Tag, prop.Name, result);
                control.PropertyChanged += GControl_PropertyChanged;
                newControls.Add(control);
            }
            PositionControls = newControls;
        }

        private void GControl_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
