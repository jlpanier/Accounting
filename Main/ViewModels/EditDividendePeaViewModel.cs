using Business;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace Main.ViewModels
{
    public class EditDividendePeaViewModel : BaseViewModel
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

                var transaction = account.Transactions.FirstOrDefault(_ => _.Id == Key && _.GetType()== typeof(Dividende));
                if (transaction is Dividende dividende)
                {
                    EffectiveOn = dividende.EffectiveOn;
                    Amount = dividende.Amount;
                    var share = Shares.FirstOrDefault(_ => _.Id == dividende.ShareId);
                    if (share != null) SelectedShare = share;
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
                    account.Dividende(SelectedShare.Id, EffectiveOn, Amount);
                }
                else if (transaction is Dividende dividende)
                {
                    dividende.Save(SelectedShare.Id, EffectiveOn, Amount);
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
                var transaction = account.Transactions.FirstOrDefault(_ => _.Id == Key && _.GetType() == typeof(Dividende));
                if (transaction is Dividende dividende)
                {
                    dividende.Delete();
                }
            }
            await Shell.Current.GoToAsync("..");
        }

    }

}
