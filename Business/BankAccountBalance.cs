using Repository.Dbo;
using Repository.Entities;

namespace Business
{
    /// <summary>
    /// Gestion des comptes bancaires
    /// </summary>
    public class BankAccountBalance
    {
        /// <summary>
        /// Création d'un compte bancaire
        /// </summary>
        public static BankAccountBalance Create(int accountNo, DateTime effectiveOn, double balance)
        {
            var item = new AccountBalanceEntity
            {
                EffectiveOn = effectiveOn,
                AccountNo= accountNo,
                Balance=balance,
                DateMaj = DateTime.Now
            };
            DatabaseAccess.Instance.Add(item);
            return new BankAccountBalance(item);
        }

        /// <summary>
        /// Référence vers l'entité de la base de données
        /// </summary>
        public readonly AccountBalanceEntity Item;

        /// <summary>
        /// Date d'effet du solde
        /// </summary>
        public DateTime EffectiveOn => Item.EffectiveOn;

        /// <summary>
        /// No du compte bancaire
        /// </summary>
        public int AccountNo => Item.AccountNo;

        /// <summary>
        /// Balance du compte
        /// </summary>
        public double Balance => Item.Balance;

        public BankAccountBalance(AccountBalanceEntity item)
        {
            Item = item;
        }

        /// <summary>
        /// Sauvegarde
        /// </summary>
        public void Save(DateTime effectiveOn, double balance)
        {
            Item.DateMaj = DateTime.Now;
            Item.EffectiveOn = effectiveOn;
            Item.Balance = balance;
            Item.DateMaj = DateTime.Now;
            DatabaseAccess.Instance.Update(Item);
        }
    }
}
