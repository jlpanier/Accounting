using Business;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Main.ViewModels
{
    /// <summary>
    /// Gestion du compte PEA
    /// </summary>
    public class EditPeaViewModel : BaseViewModel
    {
        /// <summary>
        /// Operation d'opérations du compte PEA
        /// </summary>
        public enum OperationType
        {
            Virement,
            Achat,
            Vente,
            Dividende
        }

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
        /// Evènement d'ajout d'une action
        /// </summary>
        public ICommand AddCommand => new Command(OnAdd);

        /// <summary>
        /// Collection observable pour BindableLayout
        /// </summary>
        public ObservableCollection<EditPeaViewModel> OperationHost { get; }
            = new ObservableCollection<EditPeaViewModel>();

        /// <summary>
        /// BindableLayout
        /// </summary>
        public List<OperationType> OperationTypes { get; } = Enum.GetValues(typeof(OperationType)).Cast<OperationType>().ToList();

        /// <summary>
        /// Operation d'opération sélectionné
        /// </summary>
        public OperationType SelectedOperationType
        {
            get => _selectedOperationType;
            set
            {
                if (_selectedOperationType != value)
                {
                    _selectedOperationType = value;
                    NotifyPropertyChanged(nameof(SelectedOperationType));

                    // 🔥 Force MAUI à réévaluer le DataTemplateSelector
                    OperationHost.Clear();
                    OperationHost.Add(this);
                }
            }
        }
        private OperationType _selectedOperationType;

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
                    _effectiveOn = value;
                    NotifyPropertyChanged(nameof(EffectiveOn));
                }
            }
        }
        private DateTime _effectiveOn = DateTime.Today;

        /// <summary>
        /// Montant en court
        /// </summary>
        public double Amount
        {
            get => _amount;
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    NotifyPropertyChanged(nameof(Amount));
                }
            }
        }
        private double _amount = 0;

        /// <summary>
        /// Quantité d'actions vendues ou achetées
        /// </summary>
        public double Quantity
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
        private double _quantity = 0;

        /// <summary>
        /// Quantité d'actions vendues ou achetées
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
        private double _unitPrice = 0;

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
            }
        }

        /// <summary>
        /// Sauvegarde
        /// </summary>
        private async void OnSave()
        {
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
        /// Suppression de la transaction²
        /// </summary>
        private async void OnDelete()
        {

            await Shell.Current.GoToAsync("..");
        }

        /// <summary>
        /// Ajout d'une action
        /// </summary>
        private async void OnAdd()
        {
            await Shell.Current.GoToAsync($"{nameof(SharePage)}");
        }
    }
}
