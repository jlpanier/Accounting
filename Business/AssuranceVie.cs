using Repository.Entities;

namespace Business
{
    /// <summary>
    /// Gestion des comptes bancaires
    /// </summary>
    public class AssuranceVie: BalanceAccount
    {
        public static AssuranceVie New(AccountEntity item) => new AssuranceVie(item);

        /// <summary>
        /// Création d'un compte bancaire
        /// </summary>
        public static AssuranceVie Create(string label, string accountNo, DateTime dtStart, DateTime dtEnd) => (AssuranceVie)Create(label, accountNo, dtStart, dtEnd, AccountType.AssuranceVie);

        private AssuranceVie(AccountEntity item):base(item) 
        {
        }

    }
}
