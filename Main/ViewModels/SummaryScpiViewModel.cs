using Business;
using System.Windows.Input;

namespace Main.ViewModels
{
    /// <summary>
    /// Gestion d'un compte bancaire
    /// </summary>
    public class SummaryScpiViewModel : BaseViewModel
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
        /// Nombre de parts détenues
        /// </summary>
        public int NumberOfShares
        {
            get => _numberOfShares;
            set
            {
                if (_numberOfShares != value)
                {
                    _numberOfShares = value;
                    NotifyPropertyChanged(nameof(NumberOfShares));
                }
            }
        }
        public int _numberOfShares = 0;

        /// <summary>
        /// Somme disponible à la retraite sur ce plan épargne entreprise (PEE)
        /// </summary>
        public double UnitPrice
        {
            get => _unitPrice;
            set
            {
                if (_unitPrice != value)
                {
                    _unitPrice = value;
                    NotifyPropertyChanged(nameof(UnitPrice));
                }
            }
        }
        public double _unitPrice = 0.0;

        /// <summary>
        /// Somme bloquée sur ce plan épargne entreprise (PEE)
        /// </summary>
        public double TotalPrice
        {
            get => _totalPrice;
            set
            {
                if (_totalPrice != value)
                {
                    _totalPrice = value;
                    NotifyPropertyChanged(nameof(TotalPrice));
                }
            }
        }
        public double _totalPrice = 0.0;

        /// <summary>
        /// Somme bloquée sur ce plan épargne entreprise (PEE)
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
        /// Somme bloquée sur ce plan épargne entreprise (PEE)
        /// </summary>
        public double AnnuelRent
        {
            get => _annuelRent;
            set
            {
                if (_annuelRent != value)
                {
                    _annuelRent = value;
                    NotifyPropertyChanged(nameof(AnnuelRent));
                }
            }
        }
        public double _annuelRent = 0.0;

        /// <summary>
        /// Rent sur 1 an
        /// </summary>
        public double Rendement
        {
            get => _rendement;
            set
            {
                if (_rendement != value)
                {
                    _rendement = value;
                    NotifyPropertyChanged(nameof(Rendement));
                }
            }
        }
        public double _rendement = 0.0;

        /// <summary>
        /// Date courante
        /// </summary>
        public DateTime EffectiveOn;

        #endregion

        /// <summary>
        /// Compte
        /// </summary>
        public SCPI Item
        {
            get => _item;
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
        public SCPI _item = SCPI.Empty();

        public SummaryScpiViewModel()
        {
            EffectiveOn = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        public SummaryScpiViewModel(SCPI account, DateTime dt)
        {
            Item = account;
            EffectiveOn = dt;

            NumberOfShares = 0;
            TotalPrice = 0;
            UnitPrice = 0;
            Rent = 0;
            var balance = Item.GetBalance(EffectiveOn);
            if (balance!=null)
            {
                NumberOfShares = balance.NumberOfShares;
                TotalPrice = balance.TotalPrice;
                UnitPrice = balance.UnitPrice;
                Rent = balance.Rent;
            }
            AnnuelRent = account.GetRent(EffectiveOn.AddYears(-1), EffectiveOn);
            Rendement = NumberOfShares>0 && UnitPrice>0 ? 100 * AnnuelRent / (NumberOfShares * UnitPrice): 0.0;
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
            await Shell.Current.GoToAsync($"{nameof(EditScpiPage)}", new Dictionary<string, object>
            {
                ["BankAccountId"] = Item?.BankAccountId ?? 0,
                ["EffectiveOn"] = EffectiveOn,
            });
        }
    }

}
