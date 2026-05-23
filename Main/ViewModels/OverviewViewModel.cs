using Business;
using System.ComponentModel;

namespace Main.ViewModels
{
    /// <summary>
    /// Viewmodel pour l'affichage des bilans de comptes via tempale
    /// </summary>
    public class OverviewViewModel : INotifyPropertyChanged, IBaseAccountViewModel
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler? handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
        
        /// <summary>
        /// Disponible du compte
        /// </summary>
        public double Disponible
        {
            get => _disponible;
            set
            {
                if (_disponible != value)
                {
                    _disponible = value;
                    NotifyPropertyChanged(nameof(Disponible));
                }
            }
        }
        public double _disponible;

        /// <summary>
        /// Disponible du compte
        /// </summary>
        public double Block
        {
            get => _block;
            set
            {
                if (_block != value)
                {
                    _block = value;
                    NotifyPropertyChanged(nameof(Block));
                }
            }
        }
        public double _block;

        /// <summary>
        /// Disponible du compte
        /// </summary>
        public double Retirement
        {
            get => _retirement;
            set
            {
                if (_retirement != value)
                {
                    _retirement = value;
                    NotifyPropertyChanged(nameof(Retirement));
                }
            }
        }
        public double _retirement;


        public OverviewViewModel()
        {
        }

        public OverviewViewModel(OverviewAccounts overview)
        {
            Retirement = overview.Retirement;
            Block= overview.Block;
            Disponible = overview.Disponible;
        }
    }
}
