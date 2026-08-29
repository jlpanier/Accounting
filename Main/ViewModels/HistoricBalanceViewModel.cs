using Business;
using System.Collections.ObjectModel;


namespace Main.ViewModels
{
    public class HistoricBalanceViewModel: BaseAccountViewModel
    {
        public ObservableCollection<MonthlyBalanceViewModel> Balances
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
        public ObservableCollection<MonthlyBalanceViewModel> _balances = new ObservableCollection<MonthlyBalanceViewModel>();

        /// <summary>
        /// Initialisation des données
        /// </summary>
        public void Init(int bankAccountId)
        {
            var bankAccount = BankAccount.GetById(bankAccountId);
            if (bankAccount is BalanceAccount account)
            {
                base.Init(bankAccountId, account.Label, account.AccountNo);
                Balances = new ObservableCollection<MonthlyBalanceViewModel>(MonthlyBalanceViewModel.From(account.Balances.OrderBy(_ => _.EffectiveOn)));
            }
        }
    }
}
