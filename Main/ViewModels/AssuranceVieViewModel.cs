using Business;
using System.ComponentModel;
using System.Windows.Input;

namespace Main.ViewModels
{
    /// <summary>
    /// Gestion d'un compte bancaire
    /// </summary>
    public class AssuranceVieViewModel : INotifyPropertyChanged, IBaseAccountViewModel
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
        /// Evenement de sélection d'un compte
        /// </summary>
        public ICommand SelectAccountCommand { get; }

        /// <summary>
        /// Label du compte
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
        /// Numéro du compte
        /// </summary>
        public string AccountNo
        {
            get => _accountno;
            set
            {
                if (_accountno != value)
                {
                    _accountno = value;
                    NotifyPropertyChanged(nameof(AccountNo));
                }
            }
        }
        public string _accountno = "";

        /// <summary>
        /// Balance du compte
        /// </summary>
        public double Balance
        {
            get => _balance;
            set
            {
                if (_balance != value)
                {
                    _balance = value;
                    NotifyPropertyChanged(nameof(Balance));
                }
            }
        }
        public double _balance;


        /// <summary>
        /// Compte
        /// </summary>
        public AssuranceVie? BankAccount
        {
            get => _bankAccount;
            set
            {
                if (_bankAccount != value)
                {
                    _bankAccount = value;
                    Label = _bankAccount.Label;
                    AccountNo = _bankAccount.AccountNo;
                    Balance = _bankAccount.GetBalanceOn(CurrentDate);
                    NotifyPropertyChanged(nameof(BankAccount));
                }
            }
        }
        public AssuranceVie? _bankAccount;

        /// <summary>
        /// Couleur du texte de la balance : vert si positif, rouge si négatif
        /// </summary>
        public Color BalanceColor => Balance >= 0 ? Colors.DarkGreen : Colors.DarkRed;

        public DateTime CurrentDate;

        public AssuranceVieViewModel()
        {
            SelectAccountCommand = new Command(OnAccountSelected);
            CurrentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        public AssuranceVieViewModel(AssuranceVie account, DateTime dt)
        {
            BankAccount = account;
            CurrentDate = dt;
            var item = GetBalance();
            Balance = item.Balance;
            SelectAccountCommand = new Command(OnAccountSelected);
        }

        /// <summary>
        /// Evenement de sélection d'un compte
        /// </summary>
        private async void OnAccountSelected()
        {
            var item = GetBalance();

            await Shell.Current.GoToAsync($"{nameof(NewMonthlyBalanceBankAccountPage)}", new Dictionary<string, object>
            {
                ["item"] = item
            });
       }

        private BankAccountBalance GetBalance()
        {
            var item = BankAccount.Balances.FirstOrDefault(_=>_.EffectiveOn == CurrentDate);
            if (item == null)
            {
                item = BankAccountBalance.Create(BankAccount.BankAccountId, CurrentDate, 0);
            }
            return item;
        }
    }
}
