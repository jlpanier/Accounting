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
        /// Valeur de la balance du PEE pour chaque mois
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
        /// Obtenir la balance du PEE à cette date
        /// </summary>
        public PeeBalance? GetBalance(DateTime effectiveOn) => Balances.FirstOrDefault(i => i.EffectiveOn == effectiveOn);


        public PEE()
        {
        }

        public PEE(AccountEntity item):base(item) 
        {
        }

        /// <summary>
        /// Ajout d'une item du compte bancaire
        /// </summary>
        public void AddBalance(DateTime effectiveOn, double disponible, double retirement, double blocked)
        {
            var item = Balances.FirstOrDefault(_ => _.EffectiveOn == effectiveOn);
            if (item == null)
            {
                PeeBalance.Create(BankAccountId, effectiveOn, disponible, retirement, blocked);
                _balances = null; // force le rechargement de la liste des balances
            }
            else
            {
                item.Save(disponible, retirement, blocked);
            }
        }
    }
}
