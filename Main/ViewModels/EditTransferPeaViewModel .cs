using Business;
using System.Windows.Input;

namespace Main.ViewModels
{
    public class EditTransferPeaViewModel: BaseViewModel
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
 
                var transaction = account.Transactions.FirstOrDefault(_ => _.Id == Key);
                if (transaction is Transfer transfer)
                {
                    EffectiveOn = transfer.EffectiveOn;
                    Amount = transfer.Amount;
                }
            }
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
                if (transaction is null)
                {
                    account.AddTransfer(EffectiveOn, Amount);
                }
                else if (transaction is Transfer transfer)
                {
                    transfer.Save(EffectiveOn, Amount);
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
                var transaction = account.Transactions.FirstOrDefault(_ => _.Id == Key);
                if (transaction is not null)
                {
                    transaction.Delete();
                }
            }
            await Shell.Current.GoToAsync("..");
        }

    }
}
