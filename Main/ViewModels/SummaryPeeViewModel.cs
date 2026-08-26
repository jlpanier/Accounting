using Business;
using System.Windows.Input;

namespace Main.ViewModels
{
    /// <summary>
    /// Gestion d'un compte bancaire
    /// </summary>
    public class SummaryPeeViewModel : BaseViewModel
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
        /// Somme disponible sur ce plan épargne entreprise (PEE)
        /// </summary>
        public double Disponible
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
        public double _disponible = 0.0;

        /// <summary>
        /// Somme disponible à la retraite sur ce plan épargne entreprise (PEE)
        /// </summary>
        public double Retirement
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
        public double _retirement = 0.0;

        /// <summary>
        /// Somme bloquée sur ce plan épargne entreprise (PEE)
        /// </summary>
        public double Blocked
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
        public double _blocked = 0.0;

        /// <summary>
        /// Montant total sur ce plan épargne entreprise (PEE)
        /// </summary>
        public double TotalAmount
        {
            get => _totalAmount;
            set
            {
                if (_totalAmount != value)
                {
                    _totalAmount = value;
                    NotifyPropertyChanged(nameof(TotalAmount));
                }
            }
        }
        public double _totalAmount = 0.0;

        /// <summary>
        /// Compte
        /// </summary>
        public PEE Item
        {
            get => _item;
            set
            {
                if (_item != value)
                {
                    _item = value;
                    NotifyPropertyChanged(nameof(Item));
                }
            }
        }
        public PEE _item = PEE.Empty();

        /// <summary>
        /// Date courante
        /// </summary>
        public DateTime EffectiveOn;

        #endregion

        public SummaryPeeViewModel()
        {
            EffectiveOn = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        public SummaryPeeViewModel(PEE account, DateTime dt)
        {
            Item = account;
            EffectiveOn = dt;
            Label = Item?.Label ?? string.Empty;
            AccountNo = Item?.AccountNo ?? string.Empty;

            var balance = Item?.GetBalance(EffectiveOn);
            if (balance!=null)
            {
                Disponible = balance.Disponible;
                Blocked = balance.Blocked;
                Retirement = balance.Retirement;
                TotalAmount = Disponible + Blocked + Retirement;
            }
        }

        /// <summary>
        /// Edition du compte
        /// </summary>
        private async void OnEditAccount()
        {
            await Shell.Current.GoToAsync($"{nameof(EditAccountPage)}", new Dictionary<string, object>
            {
                ["BankAccountId"] = Item?.BankAccountId ?? 0,
            });
        }

        /// <summary>
        /// Edition de la balance pour cette période
        /// </summary>
        private async void OnEditBalance()
        {
            await Shell.Current.GoToAsync($"{nameof(EditPeePage)}", new Dictionary<string, object>
            {
                ["BankAccountId"] = Item?.BankAccountId ?? 0,
                ["EffectiveOn"] = EffectiveOn,
            });
        }


    }
}
