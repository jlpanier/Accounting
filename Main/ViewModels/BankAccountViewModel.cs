using Business;
using System.ComponentModel;
using System.Windows.Input;

namespace Main.ViewModels
{
    /// <summary>
    /// Gestion d'un compte bancaire
    /// </summary>
    public class BankAccountViewModel : INotifyPropertyChanged
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
        public int AccountNo
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
        public int _accountno;

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
        /// Couleur du texte de la balance : vert si positif, rouge si négatif
        /// </summary>
        public Color BalanceColor => Balance >= 0 ? Colors.DarkGreen : Colors.DarkRed;

        public BankAccountViewModel()
        {
            Label = string.Empty;
            AccountNo = 0;
            Balance = 0;
            SelectAccountCommand = new Command<BankAccountViewModel>(OnAccountSelected);
        }

        public BankAccountViewModel(BankAccount account)
        {
            Label = account.Label;
            AccountNo = account.AccountNo;
            Balance = account.Balance;
            SelectAccountCommand = new Command<BankAccountViewModel>(OnAccountSelected);
        }

        /// <summary>
        /// Evenement de sélection d'un compte
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
