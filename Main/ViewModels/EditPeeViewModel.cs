using Business;
using System.ComponentModel;
using System.Windows.Input;

namespace Main.ViewModels
{
    public class EditPeeViewModel : BaseViewModel
    {


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
                    NotifyPropertyChanged(nameof(EffectiveOn));
                    _effectiveOn = value;
                }
            }
        }
        private DateTime _effectiveOn = DateTime.Today;

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
        private double _disponible = 0.0;

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
        private double _retirement = 0.0;

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
        private double _blocked = 0.0;

        public ICommand SaveBalancesCommand { get; }

        public EditPeeViewModel()
        {
            SaveBalancesCommand = new Command(OnSave);
        }

        /// <summary>
        /// Initialisation des données
        /// </summary>
        public void Init(PeeBalance item)
        {
            var bankAccount = BankAccount.GetByAccountId(item.BankAccountId);
            if (bankAccount != null && bankAccount.Type == BaseAccount.AccountType.PEE)
            {
                BankAccountId = item.BankAccountId;
                Label = bankAccount.Label;
                AccountNo = bankAccount.AccountNo;
                EffectiveOn = item.EffectiveOn;
                Disponible = item.Disponible;
                Blocked = item.Blocked;
                Retirement = item.Retirement;
            }
        }

        private async void OnSave()
        {
            var effectiveOn = new DateTime(EffectiveOn.Year, EffectiveOn.Month, 1);
            var bankAccount = PEE.GetByAccountId(BankAccountId);
            if (bankAccount is PEE pee)
            {
                var balance = pee.GetBalance(effectiveOn);
                balance.Save(effectiveOn, Disponible, Retirement, Blocked);
            }

            // TODO: sauvegarde dans ton repository
            await Shell.Current.GoToAsync(".."); // Retour à la page précédente
        }
    }

}
