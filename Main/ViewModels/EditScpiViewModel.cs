using Business;
using System.Windows.Input;

namespace Main.ViewModels
{
    public class EditScpiViewModel: BaseEditViewModel
    {
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
        /// Nombre de parts détenue
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
                    UpdateTotalPrice();
                }
            }
        }
        private int _numberOfShares;

        /// <summary>
        /// Prix unitaire 
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
                    UpdateTotalPrice();
                }
            }
        }
        private double _unitPrice = 0.0;

        /// <summary>
        /// Somme totale investie
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
        private double _totalPrice = 0.0;

        /// <summary>
        /// Loyer mensuelle
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
        private double _rent = 0.0;

        public EditScpiViewModel()
        {
        }

        /// <summary>
        /// Initialisation des données
        /// </summary>
        public void Init(int bankAccountId, DateTime effectiveOn)
        {
            BankAccountId = bankAccountId;
            EffectiveOn = effectiveOn;
            var account = BaseAccount.GetById(BankAccountId);
            if (account is SCPI item)
            {
                Label = item.Label;
                AccountNo = item.AccountNo;
                EffectiveOn = effectiveOn;
                var balance = item.GetBalance(effectiveOn);
                if (balance != null)
                {
                    NumberOfShares = balance.NumberOfShares;
                    UnitPrice = balance.UnitPrice;
                    Rent = balance.Rent;
                }

            }
        }

        private void UpdateTotalPrice()
        {
            TotalPrice = NumberOfShares * UnitPrice;
        }

        /// <summary>
        /// Sauvegarde de la balance mensuelle
        /// </summary>
        private async void OnSave()
        {
            var effectiveOn = new DateTime(EffectiveOn.Year, EffectiveOn.Month, 1);
            var item = SCPI.GetById(BankAccountId);
            if (item is SCPI account)
            {
                var balance = account.GetBalance(effectiveOn);
                if (balance != null)
                {
                    balance.Save(effectiveOn, NumberOfShares, UnitPrice, Rent);
                }
                else
                {
                    account.AddBalance(effectiveOn, NumberOfShares, UnitPrice, Rent);
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
            await Shell.Current.GoToAsync($"{nameof(HistoricScpiPage)}", new Dictionary<string, object>
            {
                ["BankAccountId"] = BankAccountId,
                ["EffectiveOn"] = EffectiveOn,
            });
        }
    }
}
