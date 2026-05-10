using Business;
using System.ComponentModel;
using System.Windows.Input;


namespace Main.ViewModels
{

    /// <summary>
    /// Gestion de la page principale
    /// </summary>
    public partial class MainViewModel : INotifyPropertyChanged
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

        public List<BankAccount> Accounts
        {
            get => _accounts;
            set
            {
                if (_accounts != value)
                {
                    _accounts = value;
                    NotifyPropertyChanged(nameof(Accounts));
                }
            }
        }
        public List<BankAccount>? _accounts;

        /// <summary>
        /// Ajout d'un compte
        /// </summary>
        public ICommand AddCommand { get; }

        public MainViewModel()
        {
            Accounts = new List<BankAccount>();
            AddCommand = new Command(OnClickAdd);
        }

        public async void OnClickAdd()
        {
            await Shell.Current.GoToAsync(nameof(NewBankAccountPage));
            Load();
        }

        public async void Load()
        {
            Accounts = BankAccount.GetAll();
        }
    }
}
