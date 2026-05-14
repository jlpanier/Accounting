using Business;
using System.Collections.ObjectModel;
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

        /// <summary>
        /// Evenement de suppression d'un compte bancaire
        /// </summary>
        public ICommand DeleteCommand { get; }

        /// <summary>
        /// Evenement d'édition d'un compte bancaire
        /// </summary>
        public ICommand EditCommand { get; }

        /// <summary>
        /// Liste des comptes
        /// </summary>
        public ObservableCollection<BankAccountViewModel> Accounts
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
        public ObservableCollection<BankAccountViewModel> _accounts = new ObservableCollection<BankAccountViewModel>();

        /// <summary>
        /// Ajout d'un compte
        /// </summary>
        public ICommand AddCommand { get; }

        public MainViewModel()
        {
            Accounts = new ObservableCollection<BankAccountViewModel>();
            AddCommand = new Command(OnClickAdd);
            DeleteCommand = new Command<BankAccountViewModel>(OnAccountDeleted);
            EditCommand = new Command<BankAccountViewModel>(OnAccountEdited);
        }

        /// <summary>
        /// Ajout d'un nouveau compte
        /// </summary>
        public async void OnClickAdd()
        {
            await Shell.Current.GoToAsync(nameof(NewBankAccountPage));
            Load();
        }

        /// <summary>
        /// Suppression du compte bancaire
        /// </summary>
        public void OnAccountDeleted(BankAccountViewModel vm)
        {
            vm.Delete();
            Load();
        }

        /// <summary>
        /// Modification du compte bancaire
        /// </summary>
        public async void OnAccountEdited(BankAccountViewModel vm)
        {
            await Shell.Current.GoToAsync($"{nameof(NewBankAccountPage)}", new Dictionary<string, object>
            {
                ["item"] = vm.BankAccount
            }); 
            Load();
        }

        /// <summary>
        /// Chargement des comptes
        /// </summary>
        public async void Load()
        {
            var results = new List<BankAccountViewModel>();
            var accounts = BankAccount.GetAll();
            foreach (var account in accounts)
            {
                results.Add(new BankAccountViewModel(account));
            }
            Accounts = new ObservableCollection<BankAccountViewModel>(results);
        }

        /// <summary>
        /// Sélectiopn du compte
        /// </summary>
        private async void OnAccountSelected(BankAccountViewModel account)
        {
            // Exemple : navigation, popup, édition, etc.
            await Shell.Current.GoToAsync(nameof(MonthlyBalancesPage), new Dictionary<string, object>
            {
                { "accountId", account.AccountNo }
            });
        }
    }
}
