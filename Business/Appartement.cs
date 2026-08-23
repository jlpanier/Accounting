using Repository.Dbo;
using Repository.Entities;

namespace Business
{
    public class Appartement: BaseAccount
    {
        /// <summary>
        /// Création d'une entité appartement
        /// </summary>
        public static Appartement Create(string label, string accountNo, DateTime dtStart, DateTime dtEnd)
        {
            var baseaccount = Create(label, accountNo, dtStart, dtEnd, AccountType.Appartment);
            return new Appartement(baseaccount.Item);
        }

        /// <summary>
        /// Conversion en Appartement
        /// </summary>
        public static Appartement New(AccountEntity item) => new Appartement(item);

        public Appartement()
        {
        }

        private Appartement(AccountEntity item) : base(item)
        {
        }

        /// <summary>
        /// Valeur de la balance SCPI pour chaque mois
        /// </summary>
        public List<MonthlyRent> Balances
        {
            get
            {
                if (_balances == null)
                {
                    _balances = DatabaseAccess.Instance.GetMonthlyRent(BankAccountId).Select(i => new MonthlyRent(i)).ToList();
                }
                return _balances;

            }
        }
        private List<MonthlyRent>? _balances;

        /// <summary>
        /// Obtenir le compte rendu à cette date
        /// </summary>
        public MonthlyRent? GetBalance(DateTime effectiveOn) => Balances.FirstOrDefault(i => i.EffectiveOn == effectiveOn);

        /// <summary>
        /// Ajout d'une compte rendu pour un mois donné
        /// </summary>
        public void AddBalance(DateTime effectiveOn, string renter, double rent, double charges, double inout, double work, double exceptionel, double garantee, double gestion, double syndic, double transfer)
        {
            var item = Balances.FirstOrDefault(_ => _.EffectiveOn == effectiveOn);
            if (item == null)
            {
                var monthlyrent= MonthlyRent.Create(BankAccountId, effectiveOn, renter, rent, charges, inout, work, exceptionel, garantee, gestion, syndic, transfer);
                Balances.Add(monthlyrent);
            }
            else
            {
                item.Save(effectiveOn, renter, rent, charges, inout, work, exceptionel, garantee, gestion, syndic, transfer);
            }
        }

    }
}
