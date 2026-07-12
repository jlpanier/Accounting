using Repository.Entities;

namespace Business
{
    /// <summary>
    /// Gestion des comptes bancaires
    /// </summary>
    public class BankAccount: BalanceAccount
    {
        public static BankAccount New(AccountEntity item) => new BankAccount(item);

        /// <summary>
        /// Création d'un compte bancaire
        /// </summary>
        public static BankAccount Create(string label, string accountNo, DateTime dtStart, DateTime dtEnd) => (BankAccount)Create(label, accountNo, dtStart, dtEnd, AccountType.Cheque);

        private BankAccount(AccountEntity item):base(item) 
        {
        }

    }
}
