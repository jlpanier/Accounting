using Business;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Main.ViewModels
{
    /// <summary>
    /// Gestion de la page des balances mensuelles d'un compte
    /// </summary>
    public class MonthlyBalancesViewModel : INotifyPropertyChanged
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

        #region Bindings

        /// <summary>
        /// Ajout d'un compte
        /// </summary>
        public ICommand AddCommand { get; }

        /// <summary>
        /// Reference du compte bancaire
        /// </summary>
        public BankAccount? BankAccount
        {
            get => _bankAccount;
            set
            {
                if (_bankAccount != value)
                {
                    _bankAccount = value;
                    Label = _bankAccount == null ? string.Empty : _bankAccount.Label;
                    AccountNo = _bankAccount == null ? string.Empty : _bankAccount.AccountNo.ToString();
                    NotifyPropertyChanged(nameof(BankAccount));
                }
            }
        }
        public BankAccount? _bankAccount;

        /// <summary>
        /// Label du compte bancaire
        /// </summary>
        public string Label
        {
            get => _label;
            set
            {
                if (_label != value)
                {
                    _label = value;
                    NotifyPropertyChanged(nameof(Label));
                }
            }
        }
        public string _label = "";

        /// <summary>
        /// Numéro du compte bancaire
        /// </summary>
        public string AccountNo
        {
            get => _accountNo;
            set
            {
                if (_accountNo != value)
                {
                    _accountNo = value;
                    NotifyPropertyChanged(nameof(AccountNo));
                }
            }
        }
        public string _accountNo = "";

        /// <summary>
        /// Liste des balances mensuelles du compte bancaire
        /// </summary>
        public ObservableCollection<MonthlyBalanceViewModel> MonthlyBalances
        {
            get => _monthlyBalances;
            private set
            {
                _monthlyBalances = value;
                NotifyPropertyChanged(nameof(MonthlyBalances));
            }
        }
        private ObservableCollection<MonthlyBalanceViewModel> _monthlyBalances = new ObservableCollection<MonthlyBalanceViewModel>();

        #endregion

        public MonthlyBalancesViewModel()
        {
            AddCommand = new Command(OnClickAdd);
            MonthlyBalances = new ObservableCollection<MonthlyBalanceViewModel>();
        }

        /// <summary>
        /// Ajout d'une nouvelle période 
        /// </summary>
        public async void OnClickAdd()
        {
            await Shell.Current.GoToAsync(nameof(NewMonthlyBalanceBankAccountPage), new Dictionary<string, object>
            {
                { "accountId", AccountNo }
            });
            Load();
        }

        /// <summary>
        /// Chargement des balances du compte 
        /// </summary>
        public async void Load(int accountno)
        {
            BankAccount = BankAccount.GetByAccountNo(accountno);
            Load();
        }

        /// <summary>
        /// Chargement des balances du compte 
        /// </summary>
        public async void Load()
        {
            System.Diagnostics.Debug.Assert(BankAccount != null);

            var results = new List<MonthlyBalanceViewModel>();
            var balances = BankAccount.GetBalances();
            foreach (var balance in balances)
            {
                results.Add(new MonthlyBalanceViewModel(balance));
            }
            MonthlyBalances = new ObservableCollection<MonthlyBalanceViewModel>(results.OrderByDescending(_=>_.EffectiveOn));
        }
    }
}
