using Business;
using System.Windows.Input;

namespace Main.ViewModels
{
    /// <summary>
    /// Gestion d'un compte bancaire
    /// </summary>
    public class SavingAccountViewModel : BaseViewModel
    {
        #region Propriétés

        /// <summary>
        /// Evenement pour édition du compte
        /// </summary>
        public ICommand ClickEditAccountCommand => new Command(OnEditAccount);

        /// <summary>
        /// Evenement pour édition de la balance pour cette période
        /// </summary>
        public ICommand ClickEditBalanceCommand => new Command(OnEditBalance);

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
        public SavingAccount? BankAccount
        {
            get => _bankAccount;
            set
            {
                if (_bankAccount != value)
                {
                    _bankAccount = value;
                    NotifyPropertyChanged(nameof(BankAccount));
                }
            }
        }
        public SavingAccount? _bankAccount;

        /// <summary>
        /// Date courante
        /// </summary>
        public DateTime EffectiveOn;

        #endregion

        public SavingAccountViewModel()
        {
            EffectiveOn = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        public SavingAccountViewModel(SavingAccount account, DateTime dt)
        {
            BankAccount = account;
            EffectiveOn = dt;
            Label = BankAccount?.Label ?? string.Empty;
            AccountNo = BankAccount?.AccountNo ?? string.Empty;
            Balance = BankAccount?.Balances.FirstOrDefault(_ => _.EffectiveOn == EffectiveOn)?.Balance ?? 0;
        }

        /// <summary>
        /// Edition du compte
        /// </summary>
        private async void OnEditAccount()
        {
            await Shell.Current.GoToAsync($"{nameof(EditAccountPage)}", new Dictionary<string, object>
            {
                ["BankAccountId"] = BankAccount?.BankAccountId ?? 0,
            });
        }

        /// <summary>
        /// Edition de la balance pour cette période
        /// </summary>
        private async void OnEditBalance()
        {
            await Shell.Current.GoToAsync($"{nameof(EditBalancePage)}", new Dictionary<string, object>
            {
                ["BankAccountId"] = BankAccount?.BankAccountId ?? 0,
                ["EffectiveOn"] = EffectiveOn,
            });
        }


    }
}
