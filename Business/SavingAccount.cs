using Repository.Entities;

namespace Business
{
    /// <summary>
    /// Gestion des comptes bancaires
    /// </summary>
    public class SavingAccount: BalanceAccount
    {
        /// <summary>
        /// Création d'un compte bancaire
        /// </summary>
        public static SavingAccount Create(string label, string accountNo, DateTime dtStart, DateTime dtEnd) => (SavingAccount)Create(label, accountNo, dtStart, dtEnd, AccountType.Saving);

        private SavingAccount(AccountEntity item):base(item) 
        {
        }

        public static SavingAccount New(AccountEntity item) => new SavingAccount(item);
    }
}
