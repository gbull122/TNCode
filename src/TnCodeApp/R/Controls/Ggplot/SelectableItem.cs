using System.ComponentModel;

namespace TnCode.TnCodeApp.R.Controls
{
    public class SelectableItem: INotifyPropertyChanged
    {
        private string name;
        private bool isSelected;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SelectableItem(string value,bool isSelected)
        {
            name = value;
            IsSelected = isSelected;
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }
    }
}
