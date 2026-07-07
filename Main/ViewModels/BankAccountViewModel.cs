using Business;
using System.ComponentModel;
using System.Windows.Input;

namespace Main.ViewModels
{
    /// <summary>
    /// Gestion d'un compte bancaire
    /// </summary>
    public class BankAccountViewModel : INotifyPropertyChanged, IBaseAccountViewModel
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
        /// Vrai si compte bancaire
        /// </summary>
        public bool IsAccount
        {
            get => _isAccount;
            set
            {
                if (_isAccount != value)
                {
                    _isAccount = value;
                    NotifyPropertyChanged(nameof(IsAccount));
                }
            }
        }
        public bool _isAccount = true;


        /// <summary>
        /// Compte
        /// </summary>
        public BankAccount BankAccount
        {
            get => _bankAccount;
            set
            {
                if (_bankAccount != value)
                {
                    _bankAccount = value;
                    Label = _bankAccount.Label;
                    AccountNo = _bankAccount.AccountNo;
                    Balance = _bankAccount.Balance;
                    NotifyPropertyChanged(nameof(BankAccount));
                }
            }
        }
        public BankAccount _bankAccount = BankAccount.Empty();

        /// <summary>
        /// Couleur du texte de la balance : vert si positif, rouge si négatif
        /// </summary>
        public Color BalanceColor => Balance >= 0 ? Colors.DarkGreen : Colors.DarkRed;

        public DateTime CurrentDate;

        public BankAccountViewModel()
        {
            SelectAccountCommand = new Command(OnAccountSelected);
            CurrentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        public BankAccountViewModel(BankAccount account, DateTime dt)
        {
            BankAccount = account;
            CurrentDate = dt;
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
            var item = BankAccount.Balances.FirstOrDefault(_=>_.EffectiveOn== CurrentDate);
            if (item == null)
            {
                item = BankAccountBalance.Create(BankAccount.AccountNo, CurrentDate, 0);
            }
            return item;
        }

        /// <summary>
        /// Suppression du compte bancaire
        /// </summary>
        public void Delete()
        {
            var item = BankAccount.GetByAccountNo(AccountNo);
            if (item != null)
            {
                item.Delete();
            }
        }
    }
}
