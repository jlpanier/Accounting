using Business;
using System.Windows.Input;

namespace Main.ViewModels
{
    public class EditSettingViewModel : BaseViewModel
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
        /// Référence de la configuration
        /// </summary>
        public string Key
        {
            get => _key;
            set
            {
                if (_key != value)
                {
                    _key = value;
                    NotifyPropertyChanged(nameof(Key));
                }
            }
        }
        private string _key ="";

        /// <summary>
        /// Valeur de la configuration
        /// </summary>
        public string Val
        {
            get => _val;
            set
            {
                if (_val != value)
                {
                    _val = value;
                    NotifyPropertyChanged(nameof(Val));
                }
            }
        }
        private string _val = "";

        /// <summary>
        /// Description de la configuration
        /// </summary>
        public string Desc
        {
            get => _desc;
            set
            {
                if (_desc != value)
                {
                    _desc = value;
                    NotifyPropertyChanged(nameof(Desc));
                }
            }
        }
        private string _desc = "";

        private Setting? _setting = null;
        
        public EditSettingViewModel()
        {
        }

        /// <summary>
        /// Initialisation des données
        /// </summary>
        public void Init(string key)
        {
            _setting = Settings.Instance.All.FirstOrDefault(_=>_.Key==key);
            if (_setting!=null)
            {
                Key = _setting.Key;
                Val = _setting.Val;
                Desc = _setting.Desc;
            }
        }

        /// <summary>
        /// Sauvegarde de la balance mensuelle
        /// </summary>
        private async void OnSave()
        {
            if (_setting!=null)
            {
                _setting.Save(Key, Val, Desc);
            }
            else
            {
                Settings.Instance.Add(Key, Val, Desc);
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
    }
}
