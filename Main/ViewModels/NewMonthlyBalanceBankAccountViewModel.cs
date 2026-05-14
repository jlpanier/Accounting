using Business;
using System.ComponentModel;
using System.Windows.Input;

namespace Main.ViewModels
{
    /// <summary>
    /// Gestion des balances des comptes bancaires
    /// </summary>
    public class NewMonthlyBalanceBankAccountViewModel : BaseViewModel, INotifyPropertyChanged
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
        /// Affichage du libellé du compte
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
        /// Afffichage du numéro de compte 
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
        /// Date de la lastbalance mensuelle
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
        private DateTime _effectiveOn = DateTime.Today;

        /// <summary>
        /// Balance mensuelle du compte
        /// </summary>
        public double Balance
        {
            get => _balance;
            set
            {
                if (_balance != value)
                {
                    _balance = value;
                    NotifyPropertyChanged(nameof(Balance));
                }
            }
        }
        private double _balance;


        /// <summary>
        /// Sauvegarde des données
        /// </summary>
        public ICommand SaveCommand => new Command(async () =>
        {
            try
            {
                var bankAccount = BankAccount.GetByAccountNo(AccountNo);
                if (bankAccount!=null) bankAccount.AddBalance(EffectiveOn, Balance);
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
        });

        /// <summary>
        /// Sauvegarde des données
        /// </summary>
        private async void OnValidateClicked(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Information du compte bancaire à afficher
        /// </summary>
        public void Set(int accountId)
        {
            var bankAccount = BankAccount.GetByAccountNo(accountId);
            if (bankAccount != null)
            {
                var lastbalance = bankAccount.GetBalances().OrderBy(_ => _.EffectiveOn).LastOrDefault();
                var dt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                Label = bankAccount.Label;
                AccountNo = bankAccount.AccountNo;
                EffectiveOn = DateTime.Today.Day > 15 ? dt.AddMonths(1) : dt;
                Balance = lastbalance == null ? 0 : lastbalance.Balance;
            }

        }

        /// <summary>
        /// Initialisation des données
        /// </summary>
        public void Init(BankAccountBalance item)
        {   
            Set(item.AccountNo);
            EffectiveOn = item.EffectiveOn;
            Balance = item.Balance;
        }
    }
}
