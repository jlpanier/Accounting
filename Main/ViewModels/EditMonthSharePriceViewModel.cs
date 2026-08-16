using Business;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Input;

namespace Main.ViewModels
{
    public class EditMonthSharePriceViewModel : BaseViewModel
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
        /// Actions sélectionnée pour l'achat
        /// </summary>
        public string ShareName
        {
            get => _shareName;
            set
            {
                if (_shareName != value)
                {
                    _shareName = value;
                    NotifyPropertyChanged(nameof(ShareName));
                }
            }
        }
        private string _shareName = "";

        /// <summary>
        /// Prix unitaire de l'action
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
        /// Actions sélectionnée pour l'achat
        /// </summary>
        public Business.Share? Share
        {
            get => _selectedShare;
            set
            {
                if (_selectedShare != value)
                {
                    _selectedShare = value;
                    NotifyPropertyChanged(nameof(Share));
                }
            }
        }
        private Business.Share? _selectedShare;

        /// <summary>
        /// Référence au montant de l'action à cette date
        /// </summary>
        private int PriceShareId;

        /// <summary>
        /// Initialisation des données
        /// </summary>
        public void Init(int bankAccountId, int shareId, DateTime effectiveOn)
        {
            var bankAccount = BankAccount.GetById(bankAccountId);
            if (bankAccount is PEA account)
            {
                EffectiveOn = effectiveOn;
                Share=Business.Share.GetById(shareId);
                if (Share!=null)
                {
                    ShareName = Share.Name;
                    var price = Share.GetPriceOn(EffectiveOn);
                    if (price!=null)
                    {
                        PriceShareId = price.Id;
                        UnitPrice = price.UnitPrice;
                    }
                }
            }
            CanDelete = PriceShareId > 0;
        }

        /// <summary>
        /// Sauvegarde
        /// </summary>
        private async void OnSave()
        {
            if (Share!=null)
            {
                var price = Share.GetPriceOn(EffectiveOn);
                if (price != null)
                {
                    price.Save(EffectiveOn, UnitPrice);
                }
                else
                {
                    Share.AddAmount(EffectiveOn, UnitPrice);
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
            if (Share != null)
            {
                var price = Share.GetPriceOn(EffectiveOn);
                if (price != null)
                {
                    price.Delete();
                }
            }

            await Shell.Current.GoToAsync("..");
        }

    }

}
