using Business;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Main.ViewModels
{
    /// <summary>
    /// Gestion d'un compte bancaire
    /// </summary>
    public class SummaryPeaViewModel : BaseViewModel
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
        /// Somme des virements effectuées sur ce PRA
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
        public double _transfer = 0.0;

        /// <summary>
        /// Somme disponible sur ce plan épargne entreprise (PEA)
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
        /// Dividendes sur un an de ce plan épargne entreprise (PEA)
        /// </summary>
        public double Dividendes
        {
            get => _dividendes;
            set
            {
                if (_dividendes != value)
                {
                    _dividendes = value;
                    NotifyPropertyChanged(nameof(Dividendes));
                }
            }
        }
        public double _dividendes = 0.0;
        /// <summary>
        /// Valeur titre de ce plan épargne entreprise (PEA)
        /// </summary>
        public double Valorisation
        {
            get => _valorisation;
            set
            {
                if (_valorisation != value)
                {
                    _valorisation = value;
                    NotifyPropertyChanged(nameof(Valorisation));
                }
            }
        }
        public double _valorisation = 0.0;

        /// <summary>
        /// Liste des actions déjà acquises
        /// </summary>
        public ObservableCollection<PeaGroupStatut> Groups
        {
            get => _groups;
            set
            {
                if (_groups != value)
                {
                    _groups = value;
                    NotifyPropertyChanged(nameof(Groups));
                }
            }
        }
        private ObservableCollection<PeaGroupStatut> _groups = new ObservableCollection<PeaGroupStatut>();

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

        public SummaryPeaViewModel()
        {
            EffectiveOn = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        public SummaryPeaViewModel(PEA account, DateTime dt)
        {
            Item = account;
            EffectiveOn = dt;
            PeaStatut statut = account.StatutOn(EffectiveOn);
            Cash = statut.Cash;
            Transfer = statut.Transfer;
            Groups = new ObservableCollection<PeaGroupStatut>(statut.Groups) { };
            Valorisation = statut.Groups.Select(_ => _.Valorisation).Sum();
            Dividendes = account.GetDividendes(dt.AddYears(-1), dt);
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
