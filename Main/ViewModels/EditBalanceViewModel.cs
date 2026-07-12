using Business;
using System.ComponentModel;
using System.Windows.Input;

namespace Main.ViewModels
{
    /// <summary>
    /// Gestion des balances des comptes bancaires
    /// </summary>
    public class EditBalanceViewModel : INotifyPropertyChanged, IBaseAccountViewModel
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
        /// Numéro du compte
        /// </summary>
        public int BankAccountId
        {
            get => _bankAccountId;
            set
            {
                if (_bankAccountId != value)
                {
                    _bankAccountId = value;
                    NotifyPropertyChanged(nameof(BankAccountId));
                }
            }
        }
        public int _bankAccountId;

        /// <summary>
        /// Affichage du libellé du compte
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
        private string _label = "";

        /// <summary>
        /// Afffichage du numéro de compte 
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
        private string _accountNo = "";

        /// <summary>
        /// Date de la lastbalance mensuelle
        /// </summary>
        public DateTime EffectiveOn
        {
            get => _effectiveOn;
            set
            {
                if (_effectiveOn != value)
                {
                    _effectiveOn = value;
                    NotifyPropertyChanged(nameof(EffectiveOn));
                }
            }
        }
        private DateTime _effectiveOn = DateTime.Today;

        /// <summary>
        /// Balance mensuelle du compte
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
        private double _balance;

        /// <summary>
        /// Enregistrer les soldes
        /// </summary>
        public ICommand SaveCommand { get; }

        public EditBalanceViewModel()
        {
            SaveCommand = new Command(OnSave);
        }

        /// <summary>
        /// Initialisation des données
        /// </summary>
        public void Init(BankAccountBalance item)
        {
            BankAccountId=item.BankAccountId;
            EffectiveOn = item.EffectiveOn;
            Balance = item.Balance;
        }

        /// <summary>
        /// Sauvegarde de la balance
        /// </summary>
        private async void OnSave()
        {
            var effectiveOn = new DateTime(EffectiveOn.Year, EffectiveOn.Month, 1);
            var bankAccount = BankAccount.GetByAccountId(BankAccountId);
            if (bankAccount is BalanceAccount item)
            {
                var balance = item.GetBalance(effectiveOn);
                balance.Save(effectiveOn, Balance);
            }

            // TODO: sauvegarde dans ton repository
            await Shell.Current.GoToAsync(".."); // Retour à la page précédente
        }
    }
}
