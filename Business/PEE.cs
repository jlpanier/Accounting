using Repository.Dbo;
using Repository.Entities;

namespace Business
{
    /// <summary>
    /// Gestion des comptes bancaires
    /// </summary>
    public class PEE: BaseAccount
    {
        public static PEE New(AccountEntity item) => new PEE(item);

        public new static PEE Empty() => new PEE();

        /// <summary>
        /// Chargement des item du compte bancaire
        /// </summary>
        public IEnumerable<PeeBalance> Balances
        {
            get
            {
                if (_balances == null)
                {
                    _balances = DatabaseAccess.Instance.GetMonthlyPee(BankAccountId).Select(i => new PeeBalance(i));
                }
                return _balances;

            }
        }
        private IEnumerable<PeeBalance>? _balances;

        /// <summary>
        /// Chargement des item du compte bancaire
        /// </summary>
        public PeeBalance GetBalance(DateTime effectiveOn)
        {
            var item = Balances.FirstOrDefault(i => i.EffectiveOn == effectiveOn);
            if (item == null)
            {
                item = PeeBalance.Create(BankAccountId, effectiveOn, 0, 0, 0);
            }
            return item;
        }


        public PEE()
        {
        }

        public PEE(AccountEntity item):base(item) 
        {
        }
    }
}
