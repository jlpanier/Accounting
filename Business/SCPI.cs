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
        /// Loyer annuel
        /// </summary>
        public double GetYearlyRent(DateTime effectiveOn)
        {
            var dataset = DatabaseAccess.Instance.GetMonthlyScpi(BankAccountId).Where(i => i.EffectiveOn > effectiveOn.AddYears(-1) && i.EffectiveOn <= effectiveOn).Select(i => new ScpiBalance(i));
            return dataset.Any() ? dataset.Select(_ => _.Rent).Sum() : 0.0;
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
        public ScpiBalance? GetBalance(DateTime effectiveOn)
        {
            var item = Balances.FirstOrDefault(i => i.EffectiveOn == effectiveOn);
            if (item == null || item.NumberOfShares == 0)
            {
                var previousmonth = effectiveOn.AddMonths(-1);
                var previous = Balances.FirstOrDefault(i => i.EffectiveOn == previousmonth);
                if(previous != null && previous.NumberOfShares>0)
                {
                    item = ScpiBalance.Create(BankAccountId, effectiveOn, previous.NumberOfShares, previous.UnitPrice, 0);
                }
            }
            return item;
        }

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
    }
}
