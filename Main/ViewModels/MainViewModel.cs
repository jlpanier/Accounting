using Business;
using FFImageLoading.Helpers;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace Main.ViewModels
{
    public partial class MainViewModel : BaseAccountViewModel
    {
        /// <summary>
        /// Ajout d'un compte 
        /// </summary>
        public ICommand ClickAddCommand => new Command(OnAdd);

        /// <summary>
        /// Ajout d'un compte 
        /// </summary>
        public ICommand ClickSettingsCommand => new Command(OnSettings);

        /// <summary>
        /// Révélation de la base de données 
        /// </summary>
        public ICommand ClickSendCommand => new Command(OnReveal);
        

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
                    Init(0, _currentDate.ToString("MMMM yyyy", new System.Globalization.CultureInfo("fr-FR")), $"{_currentDate.AddMonths(-1).ToString("dd/MM/yyyy")} \u2192 {_currentDate.ToString("dd/MM/yyyy")}");
                    NotifyPropertyChanged(nameof(CurrentDate));
                }
            }
        }
        private DateTime _currentDate;

        /// <summary>
        /// Label de la période 
        /// </summary>
        public double Cash
        {
            get => _cash;
            set
            {
                if (_cash != value)
                {
                    _cash = value;
                    NotifyPropertyChanged(nameof(Cash));
                }
            }
        }
        private double _cash;

        /// <summary>
        /// Epargne du mois 
        /// </summary>
        public double Saving
        {
            get => _saving;
            set
            {
                if (_saving != value)
                {
                    _saving = value;
                    NotifyPropertyChanged(nameof(Saving));
                }
            }
        }
        private double _saving;

        /// <summary>
        /// Epargne mensuel lisé sur 12 mois 
        /// </summary>
        public double MensuelSaving
        {
            get => _mensuelSaving;
            set
            {
                if (_mensuelSaving != value)
                {
                    _mensuelSaving = value;
                    NotifyPropertyChanged(nameof(MensuelSaving));
                }
            }
        }
        private double _mensuelSaving;

        /// <summary>
        /// Rente annuel 
        /// </summary>
        public double Annuity
        {
            get => _annuity;
            set
            {
                if (_annuity != value)
                {
                    _annuity = value;
                    NotifyPropertyChanged(nameof(Annuity));
                }
            }
        }
        private double _annuity;


        /// <summary>
        /// Somme disponible sur cette période
        /// </summary>
        public double Disponible
        {
            get => _disponible;
            set
            {
                if (_disponible != value)
                {
                    _disponible = value;
                    NotifyPropertyChanged(nameof(Disponible));
                }
            }
        }
        private double _disponible;

        /// <summary>
        /// Somme bloquée sur cette période
        /// </summary>
        public double Blocked
        {
            get => _blocked;
            set
            {
                if (_blocked != value)
                {
                    _blocked = value;
                    NotifyPropertyChanged(nameof(Blocked));
                }
            }
        }
        private double _blocked;

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
        private async void OnAdd()
        {
            await Shell.Current.GoToAsync(nameof(EditAccountPage));
            Load();
        }

        /// <summary>
        /// Ajout d'un compte    
        /// </summary>
        private async void OnSettings()
        {
            await Shell.Current.GoToAsync(nameof(SettingsPage));
        }

        /// <summary>
        /// Révélation de la base de données   
        /// </summary>
        private async void OnReveal()
        {
            try
            { 
                if (App.Current is App application)
                {
                    var destPath = Path.Combine(new DownloadFolderService().GetDownloadFolder(), Path.GetFileName(application.DbFilePath));

                    using (var source = File.OpenRead(application.DbFilePath))
                    {
                        using (var dest = File.OpenWrite(destPath))
                        {
                            await source.CopyToAsync(dest);
                        }
                    }
                }
                await ServiceHelper.GetService<IAlertService>()!.ShowAlertAsync(Settings.Instance.DlgDownloads);
            }
            catch (Exception ex)
            {
                await ServiceHelper.GetService<IAlertService>()!.ShowAlertAsync(ex);
            }
        }

        /// <summary>
        /// Chargement des comptes
        /// </summary>
        public async void Load()
        {
            var results = new List<IBaseAccountViewModel>();
            foreach (var item in BaseAccount.Accounts)
            {
                if (item is BankAccount account) results.Add(new SummaryBankAccountViewModel(account.BankAccountId, CurrentDate));
                else if (item is SavingAccount savingaccount) results.Add(new SummarySavingAccountViewModel(savingaccount, CurrentDate));
                else if (item is AssuranceVie assurancevie) results.Add(new SummaryAssuranceVieViewModel(assurancevie, CurrentDate));
                else if (item is PEE pee) results.Add(new SummaryPeeViewModel(pee, CurrentDate));
                else if (item is PEA pea) results.Add(new SummaryPeaViewModel(pea, CurrentDate));
                else if (item is SCPI scpi) results.Add(new SummaryScpiViewModel(scpi, CurrentDate));
                else if (item is Appartement appartement) results.Add(new SummaryRentViewModel(appartement, CurrentDate));
            }
            Accounts = new ObservableCollection<IBaseAccountViewModel>(results);

            Cash = 0;
            Disponible = 0;
            Blocked = 0;
            Annuity = 0;
            var previousMensuelCash = 0.0;
            var previousAnnuelCash = 0.0;
            foreach (var item in BaseAccount.Accounts)
            {
                if (item is BankAccount bankAccount) 
                {
                    Cash += bankAccount.GetBalanceOn(CurrentDate);
                    previousMensuelCash += bankAccount.GetBalanceOn(CurrentDate.AddMonths(-1));
                    previousAnnuelCash += bankAccount.GetBalanceOn(CurrentDate.AddYears(-1));
                    Disponible += bankAccount.GetBalanceOn(CurrentDate);
                }
                else if (item is SavingAccount savingAccount)
                {
                    Cash += savingAccount.GetBalanceOn(CurrentDate);
                    previousMensuelCash += savingAccount.GetBalanceOn(CurrentDate.AddMonths(-1));
                    previousAnnuelCash += savingAccount.GetBalanceOn(CurrentDate.AddYears(-1));
                    Disponible += savingAccount.GetBalanceOn(CurrentDate);
                }
                else if (item is Appartement appartement)
                {
                    Annuity += appartement.GetTransfer(CurrentDate.AddYears(-1), CurrentDate);
                }
                else if (item is AssuranceVie assuranceVie)
                {
                    Blocked += assuranceVie.GetBalanceOn(CurrentDate);
                }
                else if (item is PEA pea)
                {
                    var statut = pea.StatutOn(CurrentDate);
                    Blocked += statut.TotalAmount;
                    Annuity += pea.GetDividendes(CurrentDate.AddYears(-1), CurrentDate);
                }
                else if (item is PEE pee)
                {
                    var balance = pee.GetBalance(CurrentDate);
                    if (balance!=null)
                    {
                        Disponible += balance.Disponible;
                        Blocked += balance.Blocked + balance.Retirement;
                    }
                }
                else if (item is SCPI scpi)
                {
                    Annuity += scpi.GetRent(CurrentDate.AddYears(-1), CurrentDate);
                }
            }

            Saving = Cash - previousMensuelCash;
            MensuelSaving = (Cash - previousAnnuelCash)/12;
        }
    }
}
