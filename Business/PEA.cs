using Repository.Dbo;
using Repository.Entities;

namespace Business
{
    /// <summary>
    /// Gestion des comptes PEA
    /// </summary>
    public class PEA: BaseAccount
    {
        public static PEA New(AccountEntity item) => new PEA(item);

        public new static PEA Empty() => new PEA();

        /// <summary>
        /// Valeur de la balance du PEE pour chaque mois
        /// </summary>
        public IEnumerable<PeaBalance> Balances => DatabaseAccess.Instance.GetMonthlyPea(BankAccountId).Select(i => new PeaBalance(i));

        /// <summary>
        /// Obtenir la balance du PEE à cette date
        /// </summary>
        public PeaBalance GetBalance(DateTime effectiveOn)
        {
            var item = Balances.FirstOrDefault(i => i.EffectiveOn == effectiveOn);
            if (item == null)
            {
                item = PeaBalance.Create(BankAccountId, effectiveOn, 0, 0, 0, 0, 0, 0);
            }
            return item;
        }

        public PEA()
        {
        }

        public PEA(AccountEntity item):base(item) 
        {
        }
    }
}
