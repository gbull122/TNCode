using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using TnCode.Core.Data;

namespace TnCode.TnCodeApp.Data
{
    public class SelectableDataSet:IDataSet, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private IDataSet dataset;

        private bool isSelected = false;

        public bool IsSelected
        {
            get { return isSelected; }
            set 
            { 
                isSelected = value;
                RaisePropertyChanged(nameof(IsSelected));
            }
        }

        private ObservableCollection<IVariable> selectableVariables = new ObservableCollection<IVariable>();

        public SelectableDataSet(IDataSet data)
        {
            dataset = data;
            foreach(IVariable vari in data.Variables)
            {
                selectableVariables.Add(new SelectableVariable(vari));
            }
        }

        public string Name { get => dataset.Name; set => dataset.Name=value; }

        public List<string> ObservationNames => dataset.ObservationNames;

        public ObservableCollection<IVariable> Variables => selectableVariables;

        public List<string> VariableNames()
        {
            return dataset.VariableNames();
        }

        public bool AllColumnsEqual()
        {
            return dataset.AllColumnsEqual();
        }

        public IReadOnlyList<IReadOnlyList<object>> RawData()
        {
            return dataset.RawData();
        }
    }
}
