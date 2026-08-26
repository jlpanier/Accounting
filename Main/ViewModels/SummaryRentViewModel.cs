using Business;
using System.Windows.Input;

namespace Main.ViewModels
{
    /// <summary>
    /// Gestion des comptes rendus d'un appartement
    /// </summary>
    public class SummaryRentViewModel: BaseViewModel
    {
        #region Propriétés

        /// <summary>
        /// Evenement pour édition du compte
        /// </summary>
        public ICommand ClickEditAccountCommand => new Command(OnEditAccount);

        /// <summary>
        /// Evenement pour édition de la previous pour cette période
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
        /// Occupant de l'appartement
        /// </summary>
        public string Renter
        {
            get => _renter;
            set
            {
                if (_renter != value)
                {
                    _renter = value;
                    NotifyPropertyChanged(nameof(Renter));
                }
            }
        }
        public string _renter = "";

        /// <summary>
        /// Loyer perçu sur la période
        /// </summary>
        public double Rent
        {
            get => _rent;
            set
            {
                if (_rent != value)
                {
                    _rent = value;
                    NotifyPropertyChanged(nameof(Rent));
                }
            }
        }
        public double _rent = 0.0;

        /// <summary>
        /// Charge de l'appartement sur la période
        /// </summary>
        public double Provision
        {
            get => _charges;
            set
            {
                if (_charges != value)
                {
                    _charges = value;
                    NotifyPropertyChanged(nameof(Provision));
                }
            }
        }
        public double _charges = 0;

        /// <summary>
        /// Frais entrée/sortie sur la période (loyer - charges)
        /// </summary>
        public double InOut
        {
            get => _inout;
            set
            {
                if (_inout != value)
                {
                    _inout = value;
                    NotifyPropertyChanged(nameof(InOut));
                }
            }
        }
        public double _inout = 0.0;

        /// <summary>
        /// Travaux réalisés sur la période
        /// </summary>
        public double Charge
        {
            get => _work;
            set
            {
                if (_work != value)
                {
                    _work = value;
                    NotifyPropertyChanged(nameof(Charge));
                }
            }
        }
        public double _work = 0.0;

        /// <summary>
        /// Frais de garantee sur la période
        /// </summary>
        public double Garantee
        {
            get => _garantee;
            set
            {
                if (_garantee != value)
                {
                    _garantee = value;
                    NotifyPropertyChanged(nameof(Garantee));
                }
            }
        }
        public double _garantee = 0.0;

        /// <summary>
        /// Frais de gestion sur la période
        /// </summary>
        public double Gestion
        {
            get => _gestion;
            set
            {
                if (_gestion != value)
                {
                    _gestion = value;
                    NotifyPropertyChanged(nameof(Gestion));
                }
            }
        }
        public double _gestion = 0.0;

        /// <summary>
        /// Fraisdu syndic sur la période
        /// </summary>
        public double Syndic
        {
            get => _syndic;
            set
            {
                if (_syndic != value)
                {
                    _syndic = value;
                    NotifyPropertyChanged(nameof(Syndic));
                }
            }
        }
        public double _syndic = 0.0;

        /// <summary>
        /// Frais exceptionnels sur la période (travaux + charges + frais de gestion)
        /// </summary>
        public double Exceptionel
        {
            get => _exceptionel;
            set
            {
                if (_exceptionel != value)
                {
                    _exceptionel = value;
                    NotifyPropertyChanged(nameof(Exceptionel));
                }
            }
        }
        public double _exceptionel = 0.0;

        /// <summary>
        /// Transfer
        /// </summary>
        public double Transfer
        {
            get => _transfer;
            set
            {
                if (_transfer != value)
                {
                    _transfer = value;
                    NotifyPropertyChanged(nameof(Transfer));
                }
            }
        }
        public double _transfer;

        /// <summary>
        /// Solde du mois en cours
        /// </summary>
        public double Mouvement
        {
            get => _mouvement;
            set
            {
                if (_mouvement != value)
                {
                    _mouvement = value;
                    NotifyPropertyChanged(nameof(Mouvement));
                }
            }
        }
        public double _mouvement;

        /// <summary>
        /// Somme des virements/paiements sur une année
        /// </summary>
        public double AnnualTransfer
        {
            get => _annualTransfer;
            set
            {
                if (_annualTransfer != value)
                {
                    _annualTransfer = value;
                    NotifyPropertyChanged(nameof(AnnualTransfer));
                }
            }
        }
        public double _annualTransfer;


        /// <summary>
        /// Date courante
        /// </summary>
        public DateTime EffectiveOn;

        /// <summary>
        /// Référence du compte bancaire de l'appartement
        /// </summary>
        public int BankAccountId;

        #endregion

        /// <summary>
        /// Compte
        /// </summary>
        public Appartement Item
        {
            get => _item ?? new Appartement();
            set
            {
                if (_item != value)
                {
                    _item = value;
                    Label = _item.Label;
                    AccountNo = _item.AccountNo;
                    NotifyPropertyChanged(nameof(Item));
                }
            }
        }
        public Appartement? _item;

        public SummaryRentViewModel()
        {
            EffectiveOn = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        public SummaryRentViewModel(Appartement account, DateTime effectiveOn)
        {
            Item = account;
            EffectiveOn = effectiveOn;
            BankAccountId = account.BankAccountId;

            Provision = 0;
            Charge = 0;
            InOut = 0;
            Rent = 0;
            var oneyearago = EffectiveOn.AddYears(-1);
            var balance = Item.GetBalance(EffectiveOn);
            if (balance != null)
            {
                Renter = balance.Renter;
                Rent = balance.Rent + balance.Provision;
                Charge = balance.Work + balance.InOut + balance.Garantee+ balance.Gestion+ balance.Syndic + balance.Exceptionel;
                Transfer = balance.Transfer;
            }
            AnnualTransfer = account.GetTransfer(oneyearago, EffectiveOn);
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
        /// Edition de la previous pour cette période
        /// </summary>
        private async void OnEditBalance()
        {
            await Shell.Current.GoToAsync($"{nameof(EditRentPage)}", new Dictionary<string, object>
            {
                ["BankAccountId"] = Item?.BankAccountId ?? 0,
                ["EffectiveOn"] = EffectiveOn,
            });
        }



    }
}
