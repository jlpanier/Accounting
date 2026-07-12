using System.ComponentModel;

namespace Main.ViewModels
{
    public abstract partial class BaseViewModel: INotifyPropertyChanged, IBaseAccountViewModel
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler? handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
