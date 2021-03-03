using System.Collections.Generic;
using System.ComponentModel;
using TnCode.Core.Data;

namespace TnCode.TnCodeApp.Data
{
    public interface ISelectableVariable
    {
        bool IsSelected { get; set; }
    }

    public class SelectableVariable : ISelectableVariable,IVariable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private IVariable variable;
        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set 
            { 
                isSelected = value;
                RaisePropertyChanged(nameof(IsSelected));
            }
        }

        public SelectableVariable(IVariable vari)
        {
            variable = vari;
            isSelected = false;
        }

        public IReadOnlyCollection<object> Values => variable.Values;

        public int Length => variable.Length;

        public string Name => variable.Name;

        public Variable.Format DataFormat => variable.DataFormat;
    }
}
