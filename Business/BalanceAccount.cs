using Repository.Dbo;
using Repository.Entities;

namespace Business
{
    /// <summary>
    /// Gestion des comptes bancaires
    /// </summary>
    public class BalanceAccount: BaseAccount
    {
        /// <summary>
        /// Balance du compte
        /// </summary>
        public double GetBalanceOn(DateTime dt)
        {
            var result = 0.0;
            if (Balances.Any())
            {
                var item = Balances.FirstOrDefault(i=>i.EffectiveOn == dt);
                if (item != null)
                {
                    result = item.Balance;
                }
            }
            return result;
        }

        /// <summary>
        /// Chargement des item du compte bancaire
        /// </summary>
        public IEnumerable<BankAccountBalance> Balances
        {
            get 
            {
                if (_balances == null)
                {
                    _balances = DatabaseAccess.Instance.GetMonthlyBalances(BankAccountId).Select(i => new BankAccountBalance(i));
                }
                return _balances;

            }
            private set
            {
                _balances = value;
            }
        }
        private IEnumerable<BankAccountBalance>? _balances;

        /// <summary>
        /// Chargement de la balance du compte
        /// </summary>
        public BankAccountBalance GetBalance(DateTime effectiveOn)
        {
            var item = Balances.FirstOrDefault(i => i.EffectiveOn == effectiveOn);
            if (item == null)
            {
                item = BankAccountBalance.Create(BankAccountId, effectiveOn, 0);
            }
            return item;
        }

        protected BalanceAccount(AccountEntity item) : base(item)
        {
        }

        /// <summary>
        /// Ajout d'une item du compte bancaire
        /// </summary>
        public BankAccountBalance Save(DateTime effectiveOn, double balance)
        {
            var item = Balances.FirstOrDefault(_=>_.EffectiveOn == effectiveOn);
            if (item == null)
            {
                item = BankAccountBalance.Create(BankAccountId, effectiveOn, balance);
            }
            else
            {
                item.Save(effectiveOn,  balance);
            }
            Balances = DatabaseAccess.Instance.GetMonthlyBalances(BankAccountId).Select(i => new BankAccountBalance(i));
            return item;
        }
    }
}
