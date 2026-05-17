using System.Collections.ObjectModel;
using System.ComponentModel;
using static Business.BankAccount;
using System.Windows.Input;

namespace Main.ViewModels
{
    /// <summary>
    /// Affichage et sélection d'un type de compte bancaire
    /// </summary>
    public class SelectTypeAccountViewModel: BaseViewModel, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler? handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        /// <summary>
        /// Sauvegarde des données
        /// </summary>
        public ICommand SaveCommand { get; set; }

        /// <summary>
        /// Liste des types de comptes
        /// </summary>
        public ObservableCollection<AccountType> AccountTypes 
        { 
            get
            {
                if (_accountTypes==null)
                {
                    var accountTypes = new List<AccountType>();
                    foreach (AccountType accountType in Enum.GetValues(typeof(AccountType)))
                    {
                        accountTypes.Add(accountType);
                    }
                    _accountTypes = new ObservableCollection<AccountType>(accountTypes);
                }
                return _accountTypes;
            }
        } 
        private ObservableCollection<AccountType>? _accountTypes = null;

        /// <summary>
        /// Type de compte
        /// </summary>
        public AccountType SelectedAccountType
        {
            get => _selectedAccountType;
            set
            {
                if (_selectedAccountType != value)
                {
                    _selectedAccountType = value;
                    NotifyPropertyChanged(nameof(SelectedAccountType));
                }
            }
        }
        private AccountType _selectedAccountType = AccountType.Cheque;

        public SelectTypeAccountViewModel()
        {
            SaveCommand = new Command(OnSave);
        }

        /// <summary>
        /// Initialisation du type de compte au démarage
        /// </summary>
        public void Init(AccountType accountType)
        {
            SelectedAccountType = accountType;
        }

        /// <summary>
        /// Sauvegarde des données
        /// </summary>
        public async void OnSave()
        {
            try
            {
                await Shell.Current.GoToAsync("..", new Dictionary<string, object>
                {
                    { "accounttype", SelectedAccountType }
                }); // Retour à la page précédente
            }
            catch (Exception ex)
            {
                // Préférer l'utilisation de la fenêtre courante (Windows[0].Page) plutôt que Application.Current.MainPage (obsolète).
                var app = Application.Current;
                var page = app?.Windows?.FirstOrDefault()?.Page as Page;

                if (page != null)
                {
                    // Si la navigation est disponible, afficher la popup modale
                    if (page.Navigation != null)
                    {
                        await page.Navigation.PushModalAsync(new SimplePopupPage(ex.Message));
                        return;
                    }

                    // Sinon, afficher une alerte simple
                    await page.DisplayAlertAsync("Erreur", ex.Message, "OK");
                    return;
                }

                // Si aucune fenêtre/page disponible, consigner l'erreur (évite les déréférencements null)
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

    }
}
