using Business;
using System.ComponentModel;
using System.Windows.Input;

namespace Main.ViewModels
{
    /// <summary>
    /// Gestion d'un compte bancaire
    /// </summary>
    public class PeeViewModel : INotifyPropertyChanged, IBaseAccountViewModel
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
        public double Block
        {
            get => _block;
            set
            {
                if (_block != value)
                {
                    _block = value;
                    NotifyPropertyChanged(nameof(Block));
                }
            }
        }
        public double _block = 0.0;

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
                    Label = _item.Label;
                    AccountNo = _item.AccountNo;
                    NotifyPropertyChanged(nameof(Item));
                }
            }
        }
        public PEE _item = PEE.Empty();

        /// <summary>
        /// Couleur du texte de la balance : vert si positif, rouge si négatif
        /// </summary>
        public Color BalanceColor => Disponible >= 0 ? Colors.DarkGreen : Colors.DarkRed;

        public DateTime CurrentDate;

        public PeeViewModel()
        {
            SelectAccountCommand = new Command(OnAccountSelected);
            CurrentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        public PeeViewModel(PEE account, DateTime dt)
        {
            Item = account;
            CurrentDate = dt;
            SelectAccountCommand = new Command(OnAccountSelected);
        }

        /// <summary>
        /// Evenement de sélection d'un compte : affichage de la page d'édition de la balance
        /// </summary>
        private async void OnAccountSelected()
        {
            await Shell.Current.GoToAsync($"{nameof(EditPeePage)}", new Dictionary<string, object>
            {
                ["item"] = Item.GetBalance(CurrentDate)
            });
        }


    }
}
