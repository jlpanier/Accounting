using Business;
using System.Windows.Input;
using static Business.BaseAccount;

namespace Main.ViewModels
{
    /// <summary>
    /// Gestion des comptes bancaires
    /// </summary>
    public class EditAccountViewModel : BaseViewModel
    {
        /// <summary>
        /// Appel pour sélection du type de compte
        /// </summary>
        public ICommand TypeAccountCommand => new Command(OnTypeAccount);

        /// <summary>
        /// Sauvegarde des données
        /// </summary>
        public ICommand SaveCommand => new Command(OnSave);

        /// <summary>
        /// Sauvegarde des données
        /// </summary>
        public ICommand CancelCommand => new Command(OnCancel);

        /// <summary>
        /// Sauvegarde des données
        /// </summary>
        public ICommand DeleteCommand => new Command(OnDelete);

        /// <summary>
        /// Operation de compte
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

        /// <summary>
        /// Libellé du compte
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
        private string _label = "";

        /// <summary>
        /// Référence bancaire
        /// </summary>
        public string AccountNo
        {
            get => _accountNo;
            set
            {
                if (_accountNo != value)
                {
                    _accountNo = value;
                    NotifyPropertyChanged(nameof(AccountNo));
                }
            }
        }
        private string _accountNo = "";

        /// <summary>
        /// Date de début validation du compte
        /// </summary>
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    NotifyPropertyChanged(nameof(StartDate));
                }
            }
        }
        private DateTime _startDate = DateTime.Today;

        /// <summary>
        /// Date de fin validation du compte
        /// </summary>
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate != value)
                {
                    NotifyPropertyChanged(nameof(EndDate));
                    _endDate = value;
                }
            }
        }
        private DateTime _endDate = DateTime.Today;

        /// <summary>
        /// Référence du compte
        /// </summary>
        private int BankAccountId;
        
        public EditAccountViewModel()
        {
        }

        /// <summary>
        /// Initialisation des données
        /// </summary>
        public void Init(int bankAccountId)
        {
            BankAccountId = bankAccountId;
            var account = BankAccount.GetById(BankAccountId);
            if (account is IBaseAccounts item)
            {
                Label = item.Label;
                AccountNo = item.AccountNo;
                StartDate = item.StartOn;
                EndDate = item.EndOn;
                SelectedAccountType = item.Type;
            }
        }

        /// <summary>
        /// Initialisation des données
        /// </summary>
        public void Init(Business.BaseAccount.AccountType accountType)
        {
            SelectedAccountType = accountType;
        }

        /// <summary>
        /// Sauvegarde des données
        /// </summary>
        public async void OnTypeAccount()
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(SelectTypeAccountPage), new Dictionary<string, object>
                {
                    { "accounttype", SelectedAccountType }
                });
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

        /// <summary>
        /// Sauvegarde des données
        /// </summary>
        public async void OnSave()
        {
            try
            {
                var account = BankAccount.GetById(BankAccountId);
                if (account is IBaseAccounts item)
                {
                    item.Save(AccountNo, Label, StartDate, EndDate, SelectedAccountType);
                }
                else
                {
                    BankAccount.Create(AccountNo, Label, StartDate, EndDate, SelectedAccountType);
                }
                await Shell.Current.GoToAsync(".."); // Retour à la page précédente
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
            var account = BankAccount.GetById(BankAccountId);
            if (account is IBaseAccounts item)
            {
                item.Delete();
            }
            await Shell.Current.GoToAsync(".."); // Retour à la page précédente
        }
    }
}
