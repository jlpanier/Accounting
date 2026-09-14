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
        public string _accountno = string.Empty;


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
        public string _renter = string.Empty;

        /// <summary>
        /// Loyer perçu sur la période
        /// </summary>
        public string Rent
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
        public string _rent = string.Empty;

        /// <summary>
        /// Charge de l'appartement sur la période
        /// </summary>
        public string Provision
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
        public string _provision = string.Empty;

        /// <summary>
        /// Frais entrée/sortie sur la période (loyer - charges)
        /// </summary>
        public string InOut
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
        public string _inout = string.Empty;

        /// <summary>
        /// Travaux réalisés sur la période
        /// </summary>
        public string Work
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
        public string _work = string.Empty;

        /// <summary>
        /// Frais de garantee sur la période
        /// </summary>
        public string Garantee
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
        public string _garantee = string.Empty;

        /// <summary>
        /// Frais de gestion sur la période
        /// </summary>
        public string Gestion
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
        public string _gestion = string.Empty;

        /// <summary>
        /// Fraisdu syndic sur la période
        /// </summary>
        public string Syndic
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
        public string _syndic = string.Empty;

        /// <summary>
        /// Frais exceptionnels sur la période (travaux + charges + frais de gestion)
        /// </summary>
        public string Exceptionel
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
        public string _exceptionel = string.Empty;

        /// <summary>
        /// Date courante
        /// </summary>
        public string Transfer
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
        public string _transfer = string.Empty;

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
                    Rent = Settings.Instance.Rent.ToString("N2");
                    Provision = Settings.Instance.Provision.ToString("N2");
                    Work = "0.0";
                    InOut = "0.0";
                    Garantee = Settings.Instance.Garanty.ToString("N2");
                    Gestion = Settings.Instance.Gestion.ToString("N2");
                    Syndic = "0.0";
                    Transfer = "0.0";
                    Exceptionel = "0.0";
                    Renter = Settings.Instance.Renter;
                }
                else
                {
                    Renter = balance.Renter;
                    Rent = balance.Rent.ToString("N2");
                    Provision = balance.Provision.ToString("N2");
                    Work = balance.Work.ToString("N2");
                    InOut = balance.InOut.ToString("N2");
                    Garantee = balance.Garantee.ToString("N2");
                    Gestion = balance.Gestion.ToString("N2");
                    Syndic = balance.Syndic.ToString("N2");
                    Transfer = balance.Transfer.ToString("N2");
                    Exceptionel = balance.Exceptionel.ToString("N2");
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
                    if (double.TryParse(Rent.Replace(".", ","), out double rent)
                            && double.TryParse(Provision.Replace(".", ","), out double provision)
                            && double.TryParse(InOut.Replace(".", ","), out double inout)
                            && double.TryParse(Work.Replace(".", ","), out double work)
                            && double.TryParse(Exceptionel.Replace(".", ","), out double exceptionel)
                            && double.TryParse(Garantee.Replace(".", ","), out double garantee)
                            && double.TryParse(Gestion.Replace(".", ","), out double gestion)
                            && double.TryParse(Syndic.Replace(".", ","), out double syndic)
                            && double.TryParse(Transfer.Replace(".", ","), out double transfer)
                            )
                    {

                        var balance = item.GetBalance(EffectiveOn);
                        if (balance == null)
                        {
                            item.AddBalance(EffectiveOn, Renter, rent, provision, inout, work, exceptionel, garantee, gestion, syndic, transfer);
                        }
                        else
                        {
                            balance.Save(EffectiveOn, Renter, rent, provision, inout, work, exceptionel, garantee, gestion, syndic, transfer);
                        }
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
