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
                    PeriodLabel = $"{_currentDate.AddMonths(-1).ToString("dd/MM/yyyy")} \u2192 {_currentDate.ToString("dd/MM/yyyy")}";
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
        /// Label de la période 
        /// </summary>
        public string PeriodLabel
        {
            get => _periodLabel;
            set
            {
                if (_periodLabel != value)
                {
                    _periodLabel = value;
                    NotifyPropertyChanged(nameof(PeriodLabel));
                }
            }
        }
        private string _periodLabel = "";

        /// <summary>
        /// Label de la période 
        /// </summary>
        public double Liquidity
        {
            get => _liquidity;
            set
            {
                if (_liquidity != value)
                {
                    _liquidity = value;
                    NotifyPropertyChanged(nameof(Liquidity));
                }
            }
        }
        private double _liquidity;

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
            AddCommand = new Command(OnAdd);
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
        public async void OnAdd()
        {
            await Shell.Current.GoToAsync(nameof(EditAccountPage));
            Load();
        }

        /// <summary>
        /// Chargement des comptes
        /// </summary>
        public async void Load()
        {
            var results = new List<IBaseAccountViewModel>();
            foreach (var item in BaseAccount.Accounts)
            {
                if (item is BankAccount account) results.Add(new BankAccountViewModel(account.BankAccountId, CurrentDate));
                else if (item is SavingAccount savingaccount) results.Add(new SavingAccountViewModel(savingaccount, CurrentDate));
                else if (item is AssuranceVie assurancevie) results.Add(new AssuranceVieViewModel(assurancevie, CurrentDate));
                else if (item is PEE pee) results.Add(new PeeViewModel(pee, CurrentDate));
                else if (item is PEA pea) results.Add(new PeaViewModel(pea, CurrentDate));
                else if (item is SCPI scpi) results.Add(new ScpiViewModel(scpi, CurrentDate));
                else if (item is Appartement appartement) results.Add(new MonthlyRentViewModel(appartement, CurrentDate));
                else if (item is Overview overview) results.Add(new OverviewViewModel(overview));
            }
            Accounts = new ObservableCollection<IBaseAccountViewModel>(results);

            Liquidity = 0;
            foreach (var item in BaseAccount.Accounts)
            {
                if (item is BankAccount bankAccount) Liquidity += bankAccount.GetBalanceOn(CurrentDate);
               else  if (item is SavingAccount savingAccount) Liquidity += savingAccount.GetBalanceOn(CurrentDate);
            }
        }
    }
}
