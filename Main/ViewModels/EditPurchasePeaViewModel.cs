using Business;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Main.ViewModels
{
    public class EditPurchasePeaViewModel : BaseViewModel
    {
        /// <summary>
        /// Evènement de sauvegarde des données
        /// </summary>
        public ICommand ClickSaveCommand => new Command(OnSave);

        /// <summary>
        /// Evènement de sauvegarde des données
        /// </summary>
        public ICommand ClickCancelCommand => new Command(OnCancel);

        /// <summary>
        /// Evènement de sauvegarde des données
        /// </summary>
        public ICommand ClickDeleteCommand => new Command(OnDelete);

        /// <summary>
        /// Evènement d'ajout d'une nouvelle action
        /// </summary>
        public ICommand ClickAddCommand => new Command(OnAdd);

        /// <summary>
        /// Date de début validation 
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
        /// Montant en cours
        /// </summary>
        public string Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    NotifyPropertyChanged(nameof(Quantity));
                }
            }
        }
        private string _quantity = "";

        /// <summary>
        /// Prix unitaire de l'action
        /// </summary>
        public string UnitPrice
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
        private string _unitPrice = "";

        /// <summary>
        /// Commission sur cette transaction
        /// </summary>
        public string Fees
        {
            get => _fees;
            set
            {
                if (_fees != value)
                {
                    _fees = value;
                    NotifyPropertyChanged(nameof(Fees));
                }
            }
        }
        private string _fees = "";

        /// <summary>
        /// TVA sur cette transaction
        /// </summary>
        public string Tax
        {
            get => _tax;
            set
            {
                if (_tax != value)
                {
                    _tax = value;
                    NotifyPropertyChanged(nameof(Tax));
                }
            }
        }
        private string _tax = "";

        /// <summary>
        /// Vrai si on peut supprimer cette transaction
        /// </summary>
        public bool CanDelete
        {
            get => _canDelete;
            set
            {
                if (_canDelete != value)
                {
                    _canDelete = value;
                    NotifyPropertyChanged(nameof(CanDelete));
                }
            }
        }
        private bool _canDelete = true;

        /// <summary>
        /// Liste des actions déjà acquises
        /// </summary>
        public ObservableCollection<Business.Share> Shares
        {
            get => _shares;
            set
            {
                if (_shares != value)
                {
                    _shares = value;
                    NotifyPropertyChanged(nameof(Shares));
                }
            }
        }
        private ObservableCollection<Business.Share> _shares = new ObservableCollection<Business.Share>();

        /// <summary>
        /// Actions sélectionnée pour l'achat
        /// </summary>
        public Business.Share SelectedShare
        {
            get => _selectedShare;
            set
            {
                if (_selectedShare != value)
                {
                    _selectedShare = value;
                    NotifyPropertyChanged(nameof(SelectedShare));
                }
            }
        }
        private Business.Share _selectedShare = new Business.Share();

        

        /// <summary>
        /// Référence du compte
        /// </summary>
        private int BankAccountId;

        /// <summary>
        /// Référence de la transaction existante
        /// </summary>
        private int Key;

        /// <summary>
        /// Initialisation des données
        /// </summary>
        public void Init(int bankAccountId, DateTime effectiveOn, int key = 0)
        {
            var bankAccount = BankAccount.GetById(bankAccountId);
            if (bankAccount is PEA account)
            {
                BankAccountId = bankAccountId;
                EffectiveOn = effectiveOn;
                Key = key;
                CanDelete = key > 0;

                Shares = new ObservableCollection<Business.Share>(Business.Share.All);

                var transaction = account.Transactions.FirstOrDefault(_ => _.Id == Key);
                if (transaction is Order transfer)
                {
                    EffectiveOn = transfer.EffectiveOn;
                    Fees = transfer.Fees.ToString();
                    Tax = transfer.Tax.ToString();
                    Quantity = transfer.Quantity.ToString();
                    UnitPrice=transfer.UnitPrice.ToString();
                }
            }
        }

        /// <summary>
        /// Sauvegarde
        /// </summary>
        private async void OnAdd()
        {
            await Shell.Current.GoToAsync($"{nameof(SharePage)}", new Dictionary<string, object>
            {
            });
        }

        /// <summary>
        /// Sauvegarde
        /// </summary>
        private async void OnSave()
        {
            var bankAccount = BankAccount.GetById(BankAccountId);
            if (bankAccount is PEA account)
            {
                var transaction = account.Transactions.FirstOrDefault(_ => _.Id == Key);
                if (double.TryParse(Quantity.Replace('.',','), out double quantity)
                    && double.TryParse(UnitPrice.Replace('.', ','), out double unitprice)
                    && double.TryParse(Fees.Replace('.', ','), out double fees)
                    && double.TryParse(Tax.Replace('.', ','), out double tax))
                {
                    if (transaction is null)
                    {
                        account.Purchase(SelectedShare.Id, EffectiveOn, quantity, unitprice, fees, tax);
                    }
                    else if (transaction is Order transfer)
                    {
                        transfer.Save(EffectiveOn, quantity, unitprice, fees, tax);
                    }
                }
            }
            await Shell.Current.GoToAsync("..");
        }

        /// <summary>
        /// Annuler
        /// </summary>
        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        /// <summary>
        /// Suppression de la transaction
        /// </summary>
        private async void OnDelete()
        {

            var bankAccount = BankAccount.GetById(BankAccountId);
            if (bankAccount is PEA account)
            {
                var transaction = account.Transactions.FirstOrDefault(_ => _.Id == Key && _.GetType() == typeof(Order));
                if (transaction is Order order)
                {
                    order.Delete();
                }
            }
            await Shell.Current.GoToAsync("..");
        }

    }
}
