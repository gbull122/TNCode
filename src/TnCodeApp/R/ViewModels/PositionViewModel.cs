using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using TnCode.Core.R.Charts.Ggplot;
using TnCode.Core.R.Charts.Ggplot.Layer;
using TnCode.Core.Utilities;
using TnCode.TnCodeApp.R.Controls;

namespace TnCode.TnCodeApp.R.ViewModels
{
    public class PositionViewModel:BindableBase
    {
        private readonly IXmlConverter xmlConverter;

        private ObservableCollection<IOptionControl> positionControls;

        public List<string> Positions { get; }

        public DelegateCommand<string> SelectedPositionChangedCommand { get; private set; }

        public ObservableCollection<IOptionControl> PositionControls
        {
            get { return positionControls; }
            set
            {
                positionControls = value;
                RaisePropertyChanged(nameof(PositionControls));
            }
        }

        private string selectedPosition;

        public string SelectedPosition
        {
            get => selectedPosition;
            set
            {
                selectedPosition = value;
                RaisePropertyChanged(nameof(SelectedPosition));
            }
        }
        public PositionViewModel(IXmlConverter converter)
        {
            xmlConverter = converter;

            positionControls = new ObservableCollection<IOptionControl>();

            Positions = Enum.GetNames(typeof(Ggplot.Positions)).ToList();

            SelectedPositionChangedCommand = new DelegateCommand<string>(PositionChanged);
        }

        private void PositionChanged(string obj)
        {
            throw new NotImplementedException();
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

        private Position LoadPosition(string pos)
        {
            var posXml = Properties.Resources.ResourceManager.GetObject("pos_" + pos.ToLower());
            var position = xmlConverter.ToObject<Position>(posXml.ToString());
            return position;
        }
    }
}
