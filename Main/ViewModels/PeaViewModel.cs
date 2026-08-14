using Business;
using System.Windows.Input;

namespace Main.ViewModels
{
    /// <summary>
    /// Gestion d'un compte bancaire
    /// </summary>
    public class PeaViewModel : BaseViewModel
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
        /// Somme disponible à la retraite sur ce plan épargne entreprise (PEE)
        /// </summary>
        public double InvestLibre
        {
            get => _investLibre;
            set
            {
                if (_investLibre != value)
                {
                    _investLibre = value;
                    NotifyPropertyChanged(nameof(InvestLibre));
                }
            }
        }
        public double _investLibre = 0.0;

        /// <summary>
        /// Somme bloquée sur ce plan épargne entreprise (PEE)
        /// </summary>
        public double InvestProfile
        {
            get => _investProfile;
            set
            {
                if (_investProfile != value)
                {
                    _investProfile = value;
                    NotifyPropertyChanged(nameof(InvestProfile));
                }
            }
        }
        public double _investProfile = 0.0;

        /// <summary>
        /// Somme disponible sur ce plan épargne entreprise (PEE)
        /// </summary>
        public double Investissement
        {
            get => _investissement;
            set
            {
                if (_investissement != value)
                {
                    _investissement = value;
                    NotifyPropertyChanged(nameof(Investissement));
                }
            }
        }
        public double _investissement = 0.0;

        /// <summary>
        /// Somme disponible sur ce plan épargne entreprise (PEE)
        /// </summary>
        public double TitreLibre
        {
            get => _titreLibre;
            set
            {
                if (_titreLibre != value)
                {
                    _titreLibre = value;
                    NotifyPropertyChanged(nameof(TitreLibre));
                }
            }
        }
        public double _titreLibre = 0.0;

        /// <summary>
        /// Somme disponible sur ce plan épargne entreprise (PEE)
        /// </summary>
        public double TitreProfile
        {
            get => _titreProfile;
            set
            {
                if (_titreProfile != value)
                {
                    _titreProfile = value;
                    NotifyPropertyChanged(nameof(TitreProfile));
                }
            }
        }
        public double _titreProfile = 0.0;

        /// <summary>
        /// Somme disponible sur ce plan épargne entreprise (PEE)
        /// </summary>
        public double Titre
        {
            get => _titre;
            set
            {
                if (_titre != value)
                {
                    _titre = value;
                    NotifyPropertyChanged(nameof(Titre));
                }
            }
        }
        public double _titre = 0.0;

        /// <summary>
        /// Somme disponible sur ce plan épargne entreprise (PEE)
        /// </summary>
        public double Cash
        {
            get => _cash;
            set
            {
                if (_cash != value)
                {
                    _cash = value;
                    NotifyPropertyChanged(nameof(Cash));
                }
            }
        }
        public double _cash = 0.0;

        /// <summary>
        /// Somme disponible sur ce plan épargne entreprise (PEE)
        /// </summary>
        public double Total
        {
            get => _total;
            set
            {
                if (_total != value)
                {
                    _total = value;
                    NotifyPropertyChanged(nameof(Total));
                }
            }
        }
        public double _total = 0.0;

        /// <summary>
        /// Compte
        /// </summary>
        public PEA Item
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
        public PEA _item = PEA.Empty();

        /// <summary>
        /// Date courante
        /// </summary>
        public DateTime EffectiveOn;

        #endregion

        public PeaViewModel()
        {
            EffectiveOn = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        public PeaViewModel(PEA account, DateTime dt)
        {
            Item = account;
            EffectiveOn = dt;
            //var balance = Item.GetBalance(EffectiveOn);
            //TitreProfile = balance.TitreProfile;
            //InvestProfile = balance.InvestProfile;
            //InvestLibre= balance.InvestLibre;
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
            await Shell.Current.GoToAsync($"{nameof(MonthlyPeaPage)}", new Dictionary<string, object>
            {
                ["BankAccountId"] = Item?.BankAccountId ?? 0,
                ["EffectiveOn"] = EffectiveOn,
            });
        }
    }
}
