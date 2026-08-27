using Business;
using System.Collections.ObjectModel;

namespace Main.ViewModels
{
    /// <summary>
    /// Affichage de l'historique du PEE
    /// </summary>
    public class HistoricPeeViewModel : BaseAccountViewModel
    {
        #region Propriétés

        public ObservableCollection<MonthlyPeeViewModel> Balances
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
        public ObservableCollection<MonthlyPeeViewModel> _balances;

        #endregion

        /// <summary>
        /// Initialisation des données
        /// </summary>
        public void Init(int bankAccountId)
        {
            var bankAccount = BankAccount.GetById(bankAccountId);
            if (bankAccount is PEE account)
            {
                base.Init(bankAccountId,account.Label, account.AccountNo);
                Balances = new ObservableCollection<MonthlyPeeViewModel>(MonthlyPeeViewModel.From(account.Balances.OrderBy(_ => _.EffectiveOn)));
            }
        }
    }
}
