using Prism.Commands;
using Prism.Events;
using System.Collections.Generic;
using System.ComponentModel;
using TnCode.Core.R.Charts.Ggplot.Layer;

namespace TnCode.TnCodeApp.R.Controls
{
    public class VariableControl : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private IList<string> variables;
        private AestheticValue aestheticValue;
        private IEventAggregator eventAggregator;

        public DelegateCommand ActionCommand { get; private set; }

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public VariableControl(IEventAggregator evtAgg, AestheticValue aesValue, IList<string> variableNames)
        {
            eventAggregator = evtAgg;
            aestheticValue = aesValue;
            variables = variableNames;

            OnPropertyChanged("");
            ActionCommand = new DelegateCommand(DoAction, CanDoAction);
        }

        private bool CanDoAction()
        {
            return true;
        }

        private void DoAction()
        {
            if (string.IsNullOrEmpty(aestheticValue.Entry))
                return;

            //eventAggregator.GetEvent<VariableControlActionEvent>().Publish(aestheticValue.Name);

        }

        public string PropertyName
        {
            get { return aestheticValue.Name; }
            set
            {
                aestheticValue.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public IList<string> Variables
        {
            get
            {
                return variables;
            }
            set
            {
                variables = value;
                OnPropertyChanged("Variables");
            }
        }

        public string SelectedVariable
        {
            get { return aestheticValue.Entry; }
            set
            {
                aestheticValue.Entry = value;
                OnPropertyChanged("SelectedVariable");
            }
        }

        public bool? Factor
        {
            get { return aestheticValue.IsFactor == null ? false : aestheticValue.IsFactor; }
            set
            {
                aestheticValue.IsFactor = value;
                OnPropertyChanged("Factor");
            }
        }

    }
}

