using Repository.Dbo;
using Repository.Entities;

namespace Business
{
    /// <summary>
    /// Gestion des comptes bancaires
    /// </summary>
    public class BalanceAccount: BaseAccount
    {
        #region Propriétés

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
        }
        private IEnumerable<BankAccountBalance>? _balances;

        #endregion

        protected BalanceAccount(AccountEntity item) : base(item)
        {
        }

        /// <summary>
        /// Chargement de la balance du compte
        /// </summary>
        public BankAccountBalance? GetBalance(DateTime effectiveOn) => Balances.FirstOrDefault(i => i.EffectiveOn == effectiveOn);

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
        /// Ajout d'une item du compte bancaire
        /// </summary>
        public void AddBalance(DateTime effectiveOn, double balance)
        {
            var item = Balances.FirstOrDefault(_=>_.EffectiveOn == effectiveOn);
            if (item == null)
            {
                BankAccountBalance.Create(BankAccountId, effectiveOn, balance);
                _balances = null; // force le rechargement de la liste des balances
            }
            else
            {
                item.Save(effectiveOn,  balance);
            }
        }
    }
}
