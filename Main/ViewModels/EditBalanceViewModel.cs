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
        /// Enregistrer 
        /// </summary>
        public ICommand ClickSaveCommand => new Command(OnSave);

        /// <summary>
        /// Annuler 
        /// </summary>
        public ICommand ClickCancelCommand => new Command(OnCancel);

        /// <summary>
        /// Annuler 
        /// </summary>
        public ICommand ClickHistoricCommand => new Command(OnHistory);

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
        /// CanDelete mensuelle du compte
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


        public EditBalanceViewModel()
        {
        }

        /// <summary>
        /// Initialisation des données
        /// </summary>
        public void Init(int bankAccountId, DateTime effectiveOn)
        {
            BankAccountId= bankAccountId;
            EffectiveOn = effectiveOn;
            var item = BankAccount.GetById(bankAccountId);
            if (item is BalanceAccount bankaccount)
            {
                var monthlyamount = bankaccount.Balances.FirstOrDefault(b => b.EffectiveOn == effectiveOn);
                if (monthlyamount != null)
                {
                    Balance = monthlyamount.Balance;
                }
            }
        }

        /// <summary>
        /// Sauvegarde de la balance
        /// </summary>
        private async void OnSave()
        {
            var effectiveOn = new DateTime(EffectiveOn.Year, EffectiveOn.Month, 1);
            var bankAccount = BankAccount.GetById(BankAccountId);
            if (bankAccount is BalanceAccount account)
            {
                var balance = account.GetBalance(effectiveOn);
                if (balance != null)
                {
                    balance.Save(effectiveOn, Balance);
                }
                else
                {
                    account.AddBalance(effectiveOn, Balance);
                }
            }

            // TODO: sauvegarde dans ton repository
            await Shell.Current.GoToAsync(".."); // Retour à la page précédente
        }

        /// <summary>
        /// Annuler 
        /// </summary>
        public async void OnCancel()
        {
            await Shell.Current.GoToAsync(".."); // Retour à la page précédente
        }

        /// <summary>
        /// Appel à l'historique du compte 
        /// </summary>
        public async void OnHistory()
        {
            await Shell.Current.GoToAsync($"{nameof(HistoricBalancePage)}", new Dictionary<string, object>
            {
                ["BankAccountId"] = BankAccountId,
                ["EffectiveOn"] = EffectiveOn,
            });
        }
    }
}
