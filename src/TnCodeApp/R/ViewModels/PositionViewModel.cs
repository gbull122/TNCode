﻿using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using TnCode.Core.R.Charts.Ggplot;
using TnCode.Core.R.Charts.Ggplot.Layer;
using TnCode.TnCodeApp.R.Controls;

namespace TnCode.TnCodeApp.R.ViewModels
{
    public class PositionViewModel:BindableBase
    {
        private readonly IXmlService xmlService;

        private ObservableCollection<IOptionControl> positionControls;

        public event EventHandler PositionChanged;

        private Position currentPosition;
        private bool posSet;
        public List<string> Positions { get; }

        public ObservableCollection<IOptionControl> PositionControls
        {
            get { return positionControls; }
            set
            {
                positionControls = value;
                RaisePropertyChanged(nameof(PositionControls));
            }
        }

        private string currrentPositionName;

        public string CurrentPositionName
        {
            get => currrentPositionName;
            set
            {
                currrentPositionName = value;
                SelectedPositionChanged();
                RaisePropertyChanged(nameof(CurrentPositionName));
            }
        }
        public PositionViewModel(IXmlService xService)
        {
            xmlService = xService;

            positionControls = new ObservableCollection<IOptionControl>();

            Positions = Enum.GetNames(typeof(Ggplot.Positions)).ToList();
        }

        internal void SelectedPositionChanged()
        {
            if (posSet)
                return;

            var newPos = xmlService.LoadPosition(currrentPositionName);
            currentPosition = newPos;

            SetControls();

            posSet = false;
        }

        internal void SetPosition(Position pos)
        {
            currentPosition = pos;

            posSet = true;
            CurrentPositionName = pos.Name;
            SetControls();
        }

        internal void SetControls()
        {
            var newControls = new ObservableCollection<IOptionControl>();

            foreach (var control in positionControls)
            {
                control.PropertyChanged -= VariableControl_PropertyChanged;
            }
            positionControls.Clear();

            foreach (var aProp in currentPosition.Properties)
            {
                var oControl = new OptionPropertyControl(aProp);
                oControl.SetValues(aProp.Options);
                oControl.PropertyChanged += VariableControl_PropertyChanged;
                newControls.Add(oControl);
            }

            foreach (var prop in currentPosition.Booleans)
            {
                var control = new OptionCheckBoxControl(prop);
                control.PropertyChanged += VariableControl_PropertyChanged;
                newControls.Add(control);
            }

            foreach (var prop in currentPosition.Values)
            {
                double.TryParse(prop.Value, out double result);
                var control = new OptionValueControl(prop);
                control.PropertyChanged += VariableControl_PropertyChanged;
                newControls.Add(control);
            }
            PositionControls = newControls;
        }

        private void VariableControl_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PositionChanged?.Invoke(this, new EventArgs());
        }

    }
}
