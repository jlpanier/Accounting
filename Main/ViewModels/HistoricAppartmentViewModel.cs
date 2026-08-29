using Business;
using System.Collections.ObjectModel;

namespace Main.ViewModels
{
    /// <summary>
    /// Affichage de l'historique "appartements"
    /// </summary>
    public class HistoricAppartmentViewModel : BaseViewModel
    {
        #region Propriétés

        /// <summary>
        /// Référence de l'appartement
        /// </summary>
        public string Titel
        {
            get => _titel;
            set
            {
                if (_titel != value)
                {
                    _titel = value;
                    NotifyPropertyChanged(nameof(Titel));
                }
            }
        }
        public string _titel = "";


        /// <summary>
        /// Référence du compte bancaire de l'appartement
        /// </summary>
        public int BankAccountId;

        public ObservableCollection<MonthlyAppartmentViewModel> Balances
        {
            get => _balances;
            set
            {
                if (_balances != value)
                {
                    _balances = value;
                    NotifyPropertyChanged(nameof(Balances));
                }
            }
        }
        public ObservableCollection<MonthlyAppartmentViewModel> _balances = new ObservableCollection<MonthlyAppartmentViewModel>();

        #endregion

        /// <summary>
        /// Initialisation des données
        /// </summary>
        public void Init(int bankAccountId)
        {
            var bankAccount = BankAccount.GetById(bankAccountId);
            if (bankAccount is Appartement account)
            {
                BankAccountId = account.BankAccountId;
                Titel = account.Label;
                Balances = new ObservableCollection<MonthlyAppartmentViewModel>(MonthlyAppartmentViewModel.From(account.Balances.OrderBy(_=>_.EffectiveOn)));
            }
        }

    }
}
