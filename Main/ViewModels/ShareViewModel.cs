using Business;
using Common;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static Business.Share;
using static Main.ViewModels.EditPeaViewModel;

namespace Main.ViewModels
{
    public class ShareViewModel: BaseViewModel
    {
        #region Propriétés

        /// <summary>
        /// Evenement pour sauvegarde
        /// </summary>
        public ICommand ClickSaveCommand => new Command(OnSave);

        /// <summary>
        /// Evenement pour annuler l'adition
        /// </summary>
        public ICommand ClickCancelCommand => new Command(OnCancel);

        /// <summary>
        /// Pour supprimer cette action
        /// </summary>
        public ICommand ClickDeleteCommand => new Command(OnDelete);

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
        public string Code
        {
            get => _code;
            set
            {
                if (_code != value)
                {
                    _code = value;
                    NotifyPropertyChanged(nameof(Code));
                }
            }
        }
        public string _code = "";

        /// <summary>
        /// Puis-je supprimer cette action ?
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
        public bool _canDelete = false;

        /// <summary>
        /// BindableLayout
        /// </summary>
        public List<TypeShare> ShareTypes { get; } = Enum.GetValues(typeof(Business.Share.TypeShare)).Cast<TypeShare>().ToList();

        /// <summary>
        /// Operation d'action sélectionné
        /// </summary>
        public TypeShare SelectedShareType
        {
            get => _selectedShareType;
            set
            {
                if (_selectedShareType != value)
                {
                    _selectedShareType = value;
                    NotifyPropertyChanged(nameof(SelectedShareType));
                    SelectedShareTypeLabel = _selectedShareType.GetStringValue();
                }
            }
        }
        private TypeShare _selectedShareType;

        /// <summary>
        /// Numéro du compte
        /// </summary>
        public string SelectedShareTypeLabel
        {
            get => _selectedShareTypeLabel;
            set
            {
                if (_selectedShareTypeLabel != value)
                {
                    _selectedShareTypeLabel = value;
                    NotifyPropertyChanged(nameof(SelectedShareTypeLabel));
                }
            }
        }
        public string _selectedShareTypeLabel = "";

        /// <summary>
        /// Référence du compte
        /// </summary>
        public int ShareId;

        #endregion

        public ShareViewModel()
        {
        }

        /// <summary>
        /// Initialisation des données
        /// </summary>
        public void Init(int shareId=0)
        {
            ShareId = shareId;
            CanDelete = ShareId > 0;
            var share = Business.Share.All.FirstOrDefault(i => i.Id == ShareId);
            if (share != null)
            {
                Code = share.Code;
                Label = share.Label;
                SelectedShareType = share.Type;
            }
        }

        /// <summary>
        /// Sauvegarde des données
        /// </summary>
        public async void OnSave()
        {
            var share = Business.Share.All.FirstOrDefault(i => i.Id == ShareId);
            if (share != null)
            {
                share.Save(Code, Label, SelectedShareType);
            }
            else
            {
                share = Business.Share.All.FirstOrDefault(i => i.Code == Code);
                if (share != null)
                {
                    share.Save(Code, Label, SelectedShareType);
                }
                else
                {
                    Business.Share.Create(Code, Label, SelectedShareType);
                }
            }
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
        /// Suppression
        /// </summary>
        public async void OnDelete()
        {
            var share = Business.Share.GetById(ShareId);
            if (share != null)
            {
                share.Delete();
            }
            await Shell.Current.GoToAsync(".."); // Retour à la page précédente
        }
    }
}
