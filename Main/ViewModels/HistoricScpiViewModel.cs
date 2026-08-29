using Business;
using System.Collections.ObjectModel;

namespace Main.ViewModels
{
    /// <summary>
    /// Affichage de l'historique du PEE
    /// </summary>
    public class HistoricScpiViewModel : BaseAccountViewModel
    {
        #region Propriétés

        public ObservableCollection<MonthlyScpiViewModel> Balances
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
        public ObservableCollection<MonthlyScpiViewModel> _balances = new ObservableCollection<MonthlyScpiViewModel>();

        #endregion

        /// <summary>
        /// Initialisation des données
        /// </summary>
        public void Init(int bankAccountId)
        {
            var bankAccount = BankAccount.GetById(bankAccountId);
            if (bankAccount is SCPI account)
            {
                base.Init(bankAccountId, account.Label, account.AccountNo);
                Balances = new ObservableCollection<MonthlyScpiViewModel>(MonthlyScpiViewModel.From(account.Balances.OrderBy(_ => _.EffectiveOn)));
            }
        }
    }

}
