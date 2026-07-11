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
        public ICommand AddCommand { get; }

        /// <summary>
        /// Affichage de la période précédente
        /// </summary>
        public ICommand PreviousMonthCommand { get; }

        /// <summary>
        /// Affichage de la période suivante
        /// </summary>
        public ICommand NextMonthCommand { get; }

        /// <summary>
        /// Date d'affichage de la période 
        /// </summary>
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

        /// <summary>
        /// Label de la période 
        /// </summary>
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
            AddCommand = new Command(OnAddAccount);
        }

        /// <summary>
        /// Affichage de la période précédente
        /// </summary>
        public void OnPreviousMonth()
        {
            CurrentDate = CurrentDate.AddMonths(-1);
            Load();
        }

        /// <summary>
        /// Affichage de la période suivante    
        /// </summary>
        public void OnNextMonth()
        {
            if (CurrentDate < DateTime.Now)
            {
                CurrentDate = CurrentDate.AddMonths(1);
                Load();
            }
        }

        /// <summary>
        /// Ajout d'un compte    
        /// </summary>
        public async void OnAddAccount()
        {
            await Shell.Current.GoToAsync(nameof(NewBankAccountPage));
            Load();
        }

        /// <summary>
        /// Chargement des comptes
        /// </summary>
        public async void Load()
        {
            var results = new List<IBaseAccountViewModel>();
            var items = BankAccount.GetAll(CurrentDate);
            foreach (var item in items)
            {
                if (item is BankAccount account) results.Add(new BankAccountViewModel(account, CurrentDate));
                if (item is OverviewAccounts overview) results.Add(new OverviewViewModel(overview));
            }
            Accounts = new ObservableCollection<IBaseAccountViewModel>(results);
        }
    }
}
