using Business;
using System.Windows.Input;

namespace Main.ViewModels
{
    public class EditPeeViewModel : BaseViewModel
    {
        #region Propriétés

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
        /// Date de début validation du compte
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
        /// Somme disponible sur ce plan épargne entreprise (PEE)
        /// </summary>
        public string Disponible
        {
            get => _disponible;
            set
            {
                if (_disponible != value)
                {
                    _disponible = value;
                    NotifyPropertyChanged(nameof(Disponible));
                }
            }
        }
        private string _disponible = "";

        /// <summary>
        /// Somme disponible à la retraite sur ce plan épargne entreprise (PEE)
        /// </summary>
        public string Retirement
        {
            get => _retirement;
            set
            {
                if (_retirement != value)
                {
                    _retirement = value;
                    NotifyPropertyChanged(nameof(Retirement));
                }
            }
        }
        private string _retirement = "";

        /// <summary>
        /// Somme bloquée sur ce plan épargne entreprise (PEE)
        /// </summary>
        public string Blocked
        {
            get => _blocked;
            set
            {
                if (_blocked != value)
                {
                    _blocked = value;
                    NotifyPropertyChanged(nameof(Blocked));
                }
            }
        }
        private string _blocked = "";

        #endregion

        public EditPeeViewModel()
        {
        }

        /// <summary>
        /// Initialisation des données
        /// </summary>
        public void Init(int bankAccountId, DateTime effectiveOn)
        {
            var bankAccount = BankAccount.GetById(bankAccountId);
            if (bankAccount is PEE account)
            {
                BankAccountId = account.BankAccountId;
                Label = account.Label;
                AccountNo = account.AccountNo;
                EffectiveOn = effectiveOn;
                var balance = account.GetBalance(effectiveOn);
                if (balance != null)
                {
                    Disponible = balance.Disponible.ToString();
                    Blocked = balance.Blocked.ToString();
                    Retirement = balance.Retirement.ToString();
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
            if (bankAccount is PEE account)
            {
                var balance = account.GetBalance(effectiveOn);
                if (double.TryParse(Disponible.Replace('.', ','), out double disponible)
                    && double.TryParse(Retirement.Replace('.', ','), out double retirement)
                    && double.TryParse(Blocked.Replace('.', ','), out double blocked))
                {
                    if (balance != null)
                    {
                        balance.Save(disponible, retirement, blocked);
                    }
                    else
                    {
                        account.AddBalance(effectiveOn, disponible, retirement, blocked);
                    }
                }
            }
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
            await Shell.Current.GoToAsync($"{nameof(HistoricPeePage)}", new Dictionary<string, object>
            {
                ["BankAccountId"] = BankAccountId,
                ["EffectiveOn"] = EffectiveOn,
            });
        }
    }

}
