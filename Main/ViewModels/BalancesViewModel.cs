using System;
using System.Collections.Generic;
using System.Text;

namespace Main.ViewModels
{
    using System.Collections.ObjectModel;

    public class BalancesViewModel
    {
        public ObservableCollection<MonthlyAccountBalance> MonthlyBalances { get; }

        public BalancesViewModel()
        {
            MonthlyBalances = new ObservableCollection<MonthlyAccountBalance>
        {
            new MonthlyAccountBalance { Month = "Janvier",  Account1 = 1200, Account2 = 3500, Account3 = 8000 },
            new MonthlyAccountBalance { Month = "Février",  Account1 = 900,  Account2 = 3600, Account3 = 8050 },
            new MonthlyAccountBalance { Month = "Mars",     Account1 = 1500, Account2 = 3700, Account3 = 8100 },
            new MonthlyAccountBalance { Month = "Avril",    Account1 = 1100, Account2 = 3800, Account3 = 8200 },
            // etc.
        };
        }
    }

}
