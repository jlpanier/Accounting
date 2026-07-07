using Business;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;


namespace Main.ViewModels
{
    /// <summary>
    /// Gestion de la page principale
    /// </summary>
    public partial class AccountMainViewModel : INotifyPropertyChanged
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
        public ObservableCollection<IBaseAccountViewModel> Accounts
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
        public ObservableCollection<IBaseAccountViewModel> _accounts = new ObservableCollection<IBaseAccountViewModel>();

        /// <summary>
        /// Ajout d'un compte
        /// </summary>
        public ICommand AddCommand { get; }

        public AccountMainViewModel()
        {
            Accounts = new ObservableCollection<IBaseAccountViewModel>();
            AddCommand = new Command(OnClickAdd);
            DeleteCommand = new Command<IBaseAccountViewModel>(OnAccountDeleted);
            EditCommand = new Command<IBaseAccountViewModel>(OnAccountEdited);
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
        public void OnAccountDeleted(IBaseAccountViewModel viewmodel)
        {
            if (viewmodel is BankAccountViewModel vm)  vm.Delete();
            Load();
        }

        /// <summary>
        /// Modification du compte bancaire
        /// </summary>
        public async void OnAccountEdited(IBaseAccountViewModel viewmodel)
        {
            if (viewmodel is BankAccountViewModel vm)
            {
                await Shell.Current.GoToAsync($"{nameof(NewBankAccountPage)}", new Dictionary<string, object>
                {
                    ["item"] = vm.BankAccount
                });
            }
            Load();
        }

        /// <summary>
        /// Chargement des comptes
        /// </summary>
        public async void Load()
        {
            var results = new List<IBaseAccountViewModel>();
            var items = BankAccount.GetAll();
            foreach (var item in items)
            {
                if (item is BankAccount account) results.Add(new BankAccountViewModel(account, DateTime.Now));
                if (item is OverviewAccounts overview) results.Add(new OverviewViewModel(overview));
            }
            Accounts = new ObservableCollection<IBaseAccountViewModel>(results);
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
