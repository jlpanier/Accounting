using Business;
using System.Windows.Input;

namespace Main.ViewModels
{
    /// <summary>
    /// Gestion de la location d'un appartement
    /// </summary>
    public class EditRentViewModel: BaseViewModel
    {
        #region Propriétés

        /// <summary>
        /// Enregistrer 
        /// </summary>
        public ICommand ClickSaveCommand => new Command(OnSave);

        /// <summary>
        /// Annuler 
        /// </summary>
        public ICommand ClickCancelCommand => new Command(OnCancel);

        /// <summary>
        /// Annuler 
        /// </summary>
        public ICommand ClickHistoricCommand => new Command(OnHistory);

        /// <summary>
        /// Référence de l'appartement
        /// </summary>
        public string AccountNo
        {
            get => _accountno;
            set
            {
                if (_accountno != value)
                {
                    _accountno = value;
                    NotifyPropertyChanged(nameof(AccountNo));
                }
            }
        }
        public string _accountno = "";


        /// <summary>
        /// Occupant de l'appartement
        /// </summary>
        public string Renter
        {
            get => _renter;
            set
            {
                if (_renter != value)
                {
                    _renter = value;
                    NotifyPropertyChanged(nameof(Renter));
                }
            }
        }
        public string _renter = "";

        /// <summary>
        /// Loyer perçu sur la période
        /// </summary>
        public double Rent
        {
            get => _rent;
            set
            {
                if (_rent != value)
                {
                    _rent = value;
                    NotifyPropertyChanged(nameof(Rent));
                }
            }
        }
        public double _rent = 0.0;

        /// <summary>
        /// Charge de l'appartement sur la période
        /// </summary>
        public double Provision
        {
            get => _provision;
            set
            {
                if (_provision != value)
                {
                    _provision = value;
                    NotifyPropertyChanged(nameof(Provision));
                }
            }
        }
        public double _provision = 0;

        /// <summary>
        /// Frais entrée/sortie sur la période (loyer - charges)
        /// </summary>
        public double InOut
        {
            get => _inout;
            set
            {
                if (_inout != value)
                {
                    _inout = value;
                    NotifyPropertyChanged(nameof(InOut));
                }
            }
        }
        public double _inout = 0.0;

        /// <summary>
        /// Travaux réalisés sur la période
        /// </summary>
        public double Work
        {
            get => _work;
            set
            {
                if (_work != value)
                {
                    _work = value;
                    NotifyPropertyChanged(nameof(Work));
                }
            }
        }
        public double _work = 0.0;

        /// <summary>
        /// Frais de garantee sur la période
        /// </summary>
        public double Garantee
        {
            get => _garantee;
            set
            {
                if (_garantee != value)
                {
                    _garantee = value;
                    NotifyPropertyChanged(nameof(Garantee));
                }
            }
        }
        public double _garantee = 0.0;

        /// <summary>
        /// Frais de gestion sur la période
        /// </summary>
        public double Gestion
        {
            get => _gestion;
            set
            {
                if (_gestion != value)
                {
                    _gestion = value;
                    NotifyPropertyChanged(nameof(Gestion));
                }
            }
        }
        public double _gestion = 0.0;

        /// <summary>
        /// Fraisdu syndic sur la période
        /// </summary>
        public double Syndic
        {
            get => _syndic;
            set
            {
                if (_syndic != value)
                {
                    _syndic = value;
                    NotifyPropertyChanged(nameof(Syndic));
                }
            }
        }
        public double _syndic = 0.0;

        /// <summary>
        /// Frais exceptionnels sur la période (travaux + charges + frais de gestion)
        /// </summary>
        public double Exceptionel
        {
            get => _exceptionel;
            set
            {
                if (_exceptionel != value)
                {
                    _exceptionel = value;
                    NotifyPropertyChanged(nameof(Exceptionel));
                }
            }
        }
        public double _exceptionel = 0.0;

        /// <summary>
        /// Date courante
        /// </summary>
        public double Transfer
        {
            get => _transfer;
            set
            {
                if (_transfer != value)
                {
                    _transfer = value;
                    NotifyPropertyChanged(nameof(Transfer));
                }
            }
        }
        public double _transfer;

        /// <summary>
        /// Date courante
        /// </summary>
        public DateTime EffectiveOn
        {
            get => _effectiveOn;
            set
            {
                if (_effectiveOn != value)
                {
                    _effectiveOn = value;
                    NotifyPropertyChanged(nameof(EffectiveOn));
                }
            }
        }
        public DateTime _effectiveOn;

        /// <summary>
        /// Référence du compte bancaire de l'appartement
        /// </summary>
        public int BankAccountId;

        #endregion

        /// <summary>
        /// Initialisation des données
        /// </summary>
        public void Init(int bankAccountId, DateTime effectiveOn)
        {
            var bankAccount = BankAccount.GetById(bankAccountId);
            if (bankAccount is Appartement account)
            {
                BankAccountId = account.BankAccountId;
                AccountNo = account.AccountNo;
                EffectiveOn = effectiveOn;
                var balance = account.GetBalance(effectiveOn);
                if (balance == null)
                {
                    Rent = Settings.Instance.Rent;
                    Provision = Settings.Instance.Provision;
                    Work = 0;
                    InOut = 0;
                    Garantee = Settings.Instance.Garanty;
                    Gestion = Settings.Instance.Gestion;
                    Syndic = 0;
                    Transfer = 0;
                    Exceptionel = 0;
                    Renter = Settings.Instance.Renter;
                }
                else
                {
                    Renter = balance.Renter;
                    Rent = balance.Rent;
                    Provision = balance.Provision;
                    Work = balance.Work;
                    InOut = balance.InOut;
                    Garantee = balance.Garantee;
                    Gestion = balance.Gestion;
                    Syndic = balance.Syndic;
                    Transfer = balance.Transfer;
                    Exceptionel = balance.Exceptionel;
                }
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
                if (account is Appartement item)
                {
                    var balance = item.GetBalance(EffectiveOn);
                    if (balance == null)
                    {
                        item.AddBalance( EffectiveOn, Renter, Rent, Provision, InOut, Work, Exceptionel, Garantee, Gestion, Syndic, Transfer);
                    }
                    else
                    {
                        balance.Save(EffectiveOn, Renter, Rent, Provision, InOut, Work, Exceptionel, Garantee, Gestion, Syndic, Transfer);
                    }
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
        /// Appel à l'historique du compte 
        /// </summary>
        public async void OnHistory()
        {
            await Shell.Current.GoToAsync($"{nameof(HistoricAppartmentPage)}", new Dictionary<string, object>
            {
                ["BankAccountId"] = BankAccountId,
                ["EffectiveOn"] = EffectiveOn,
            });
        }
    }
}
