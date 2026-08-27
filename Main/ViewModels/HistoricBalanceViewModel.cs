using Business;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Main.ViewModels
{
    public class HistoricBalanceViewModel: BaseAccountViewModel
    {
        #region Propriétés

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
        public ObservableCollection<MonthlyBalanceViewModel> _balances;

        #endregion

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
