using Business;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace Main.ViewModels
{
    public class SettingsViewModel : BaseAccountViewModel
    {
        /// <summary>
        /// Ajout d'une configuration 
        /// </summary>
        public ICommand ClickAddCommandd => new Command(OnAdd);

        /// <summary>
        /// Maj d'une configuration 
        /// </summary>
        public ICommand ClickUpdateCommand => new Command<SettingViewModel>(OnUpdate);

        /// <summary>
        /// Annuler 
        /// </summary>
        public ICommand ClickCancelCommand => new Command(OnCancel);

        /// <summary>
        /// Liste de la configuration
        /// </summary>
        public ObservableCollection<SettingViewModel> Items
        {
            get => _items;
            set
            {
                if (_items != value)
                {
                    _items = value;
                    NotifyPropertyChanged(nameof(Items));
                }
            }
        }
        public ObservableCollection<SettingViewModel> _items = new ObservableCollection<SettingViewModel>();

        /// <summary>
        /// Initialisation des données
        /// </summary>
        public void Load()
        {
            var items = Settings.Instance.All;
            base.Init(0, "Configuration", "-");
            Items = new ObservableCollection<SettingViewModel>(SettingViewModel.From(items));
        }

        /// <summary>
        /// Ajout 
        /// </summary>
        public async void OnAdd()
        {
            await Shell.Current.GoToAsync($"{nameof(EditSettingPage)}", new Dictionary<string, object>
            {
                ["Key"] = 0,
            });
        }

        /// <summary>
        /// Ajout 
        /// </summary>
        public async void OnUpdate(SettingViewModel item)
        {
            await Shell.Current.GoToAsync($"{nameof(EditSettingPage)}", new Dictionary<string, object>
            {
                ["Key"] = item.Key,
            });
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
