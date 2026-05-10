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
                System.Diagnostics.Debug.Assert(Application.Current != null);
                await Application.Current.MainPage.Navigation.PushModalAsync(new SimplePopupPage(ex.Message));
            }
        });

        private async void OnValidateClicked(object sender, EventArgs e)
        {
        }

    }

}
