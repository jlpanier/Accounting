using Business;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Main.ViewModels
{
    public partial class MainViewModel : INotifyPropertyChanged
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
        /// Ajout d'un compte
        /// </summary>
        public ICommand PreviousMonthCommand { get; }

        /// <summary>
        /// Ajout d'un compte
        /// </summary>
        public ICommand NextMonthCommand { get; }

        public DateTime CurrentDate
        {
            get => _currentDate;
            set
            {
                if (_currentDate != value)
                {
                    _currentDate = value;
                    MonthLabel = _currentDate.ToString("MMMM yyyy", new System.Globalization.CultureInfo("fr-FR"));
                    NotifyPropertyChanged(nameof(CurrentDate));
                }
            }
        }
        private DateTime _currentDate;

        public string MonthLabel
        {
            get => _monthLabel;
            set
            {
                if (_monthLabel != value)
                {
                    _monthLabel = value;
                    NotifyPropertyChanged(nameof(MonthLabel));
                }
            }
        }
        private string _monthLabel = "";

        /// <summary>
        /// Liste des comptes
        /// </summary>
        public ObservableCollection<IBaseAccountViewModel> Accounts
        {
            get => _accounts;
            set
            {
                if (_accounts != value)
                {
                    _accounts = value;
                    NotifyPropertyChanged(nameof(Accounts));
                }
            }
        }
        public ObservableCollection<IBaseAccountViewModel> _accounts = new ObservableCollection<IBaseAccountViewModel>();

        public MainViewModel()
        {
            Accounts = new ObservableCollection<IBaseAccountViewModel>();
            CurrentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            PreviousMonthCommand = new Command(OnPreviousMonth);
            NextMonthCommand = new Command(OnNextMonth);
        }

        /// <summary>
        /// Suppression du compte bancaire
        /// </summary>
        public void OnPreviousMonth()
        {
            CurrentDate = CurrentDate.AddMonths(-1);
        }

        /// <summary>
        /// Suppression du compte bancaire
        /// </summary>
        public void OnNextMonth()
        {
            CurrentDate = CurrentDate.AddMonths(1);
        }

        /// <summary>
        /// Chargement des comptes
        /// </summary>
        public async void Load()
        {
            var results = new List<IBaseAccountViewModel>();
            var items = BankAccount.GetAll();
            foreach (var item in items)
            {
                if (item is BankAccount account) results.Add(new BankAccountViewModel(account, CurrentDate));
                if (item is OverviewAccounts overview) results.Add(new OverviewViewModel(overview));
            }
            Accounts = new ObservableCollection<IBaseAccountViewModel>(results);
        }
    }
}
