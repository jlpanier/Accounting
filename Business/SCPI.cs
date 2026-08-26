using Repository.Dbo;
using Repository.Entities;

namespace Business
{
    /// <summary>
    /// Gestion des comptes bancaires
    /// </summary>
    public class SCPI: BaseAccount
    {
        /// <summary>
        /// Création d'un compte bancaire
        /// </summary>
        public static SCPI Create(string label, string accountNo, DateTime dtStart, DateTime dtEnd)
        {
            var baseaccount = Create(label, accountNo, dtStart, dtEnd, AccountType.SCPI);
            return new SCPI(baseaccount.Item);
        }

        /// <summary>
        /// Convertir un AccountEntity en SCPI
        /// </summary>
        public static SCPI New(AccountEntity item) => new SCPI(item);

        /// <summary>
        /// SCPI vide 
        /// </summary>
        public static SCPI Empty() => new SCPI();

        public SCPI()
        {
        }

        private SCPI(AccountEntity item):base(item) 
        {
        }

        /// <summary>
        /// Valeur de la balance SCPI pour chaque mois
        /// </summary>
        public IEnumerable<ScpiBalance> Balances
        {
            get
            {
                if (_balances == null)
                {
                    _balances = DatabaseAccess.Instance.GetMonthlyScpi(BankAccountId).Select(i => new ScpiBalance(i));
                }
                return _balances;

            }
        }
        private IEnumerable<ScpiBalance>? _balances;

        /// <summary>
        /// Obtenir la balance du SCPI à cette date
        /// </summary>
        public ScpiBalance? GetBalance(DateTime effectiveOn) => Balances.FirstOrDefault(i => i.EffectiveOn == effectiveOn);

        /// <summary>
        /// Ajout d'une item 
        /// </summary>
        public void AddBalance(DateTime effectiveOn, int numberOfShares, double unitPrice, double rente)
        {
            var item = Balances.FirstOrDefault(_ => _.EffectiveOn == effectiveOn);
            if (item == null)
            {
                ScpiBalance.Create(BankAccountId, effectiveOn, numberOfShares, unitPrice, rente);
                _balances = null; // force le rechargement de la liste des balances
            }
            else
            {
                item.Save(effectiveOn, numberOfShares, unitPrice, rente);
            }
        }

        /// <summary>
        /// Somme des loyers sur une période d'un an
        /// </summary>
        /// <param name="starton"></param>
        /// <param name="endOn"></param>
        /// <returns></returns>
        public double GetRent(DateTime starton, DateTime endOn) => Balances.Where(_ => _.EffectiveOn > starton && _.EffectiveOn < endOn).Select(_ => _.Rent).Sum();
    }
}
