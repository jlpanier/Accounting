using Business;
using System.ComponentModel;
using System.Windows.Input;

namespace Main.ViewModels
{
    /// <summary>
    /// Gestion d'un compte bancaire
    /// </summary>
    public class ScpiViewModel : INotifyPropertyChanged, IBaseAccountViewModel
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
        public ICommand SelectCommand { get; }

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
        /// Rendement sur 1 an
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

        /// <summary>
        /// Couleur du texte de la balance : vert si positif, rouge si négatif
        /// </summary>
        public Color BalanceColor => NumberOfShares >= 0 ? Colors.DarkGreen : Colors.DarkRed;

        public DateTime CurrentDate;

        public ScpiViewModel()
        {
            SelectCommand = new Command(OnSelected);
            CurrentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        public ScpiViewModel(SCPI account, DateTime dt)
        {
            Item = account;
            CurrentDate = dt;
            var balance = Item.GetBalance(CurrentDate);
            if (balance==null)
            {
                NumberOfShares = 0;
                TotalPrice = 0;
                UnitPrice = 0;
                Rent = 0;
            }
            else
            {
                NumberOfShares = balance.NumberOfShares;
                TotalPrice = balance.TotalPrice;
                UnitPrice = balance.UnitPrice;
                Rent = balance.Rent;
            }
            AnnuelRent = account.GetYearlyRent(CurrentDate);
            Rendement = TotalPrice>0 ? 100 * AnnuelRent / TotalPrice : 0.0;
            SelectCommand = new Command(OnSelected);
        }

        /// <summary>
        /// Evenement de sélection d'un compte : affichage de la page d'édition de la balance
        /// </summary>
        private async void OnSelected()
        {
            await Shell.Current.GoToAsync($"{nameof(EditScpiPage)}", new Dictionary<string, object>
            {
                ["item"] = Item,
                ["effectiveOn"] = CurrentDate

            });
        }


    }

}
