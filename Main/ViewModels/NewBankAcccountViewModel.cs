using Business;
using System.ComponentModel;
using System.Windows.Input;

namespace Main.ViewModels
{
    /// <summary>
    /// Gestion des comptes bancaires
    /// </summary>
    public class NewBankAcccountViewModel : BaseViewModel, INotifyPropertyChanged
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
        public int AccountNo
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
        private int _accountNo;

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
                    NotifyPropertyChanged(nameof(StartDate));
                    _startDate = value;
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
        /// Initialisation des données
        /// </summary>
        public void Init(BankAccount item)
        {
            Label = item.Label;
            AccountNo = item.AccountNo;
            StartDate = item.StartOn;
            EndDate = item.EndOn;
        }

        /// <summary>
        /// Sauvegarde des données
        /// </summary>
        public ICommand SaveCommand => new Command(async () =>
        {
            try
            {
                BankAccount.Update(Label.Trim(), AccountNo, StartDate, EndDate);
                await Shell.Current.GoToAsync(".."); // Retour à la page précédente
            }
            catch(Exception ex)
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
        });
    }
}
