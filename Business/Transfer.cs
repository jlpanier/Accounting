using Repository.Dbo;
using Repository.Entities;

namespace Business
{
    /// <summary>
    /// Gestion des transactions de virement
    /// </summary>
    public class Transfer: ITransaction
    {
        /// <summary>
        /// Création d'une transaction de virement
        /// </summary>
        public static Transfer Create(int bankAccountId, DateTime effectiveOn, double amount)
        {
            var item = new TranferEntity()
            {
                BankAccountId = bankAccountId,
                EffectiveOn = effectiveOn,
                Amount = amount,
                DateMaj = DateTime.Now,
            };
            DatabaseAccess.Instance.Add(item);
            return new Transfer(item);
        }

        /// <summary>
        /// Référence vers l'entité de la base de données
        /// </summary>
        public readonly TranferEntity Item;

        /// <summary>
        /// Référence vers l'entité de la base de données
        /// </summary>
        public int Id => Item.Id;

        /// <summary>
        /// Date d'effet du solde
        /// </summary>
        public DateTime EffectiveOn => Item.EffectiveOn;

        /// <summary>
        /// No du compte bancaire
        /// </summary>
        public int BankAccountId => Item.BankAccountId;

        /// <summary>
        /// Montant de la transaction
        /// </summary>
        public double Amount => Item.Amount;

        /// <summary>
        /// Libellé de la transaction
        /// </summary>
        public string Label
        {
            get
            {
                return "Virement";
            }
        }

        public Transfer(TranferEntity item)
        {
            Item = item;
        }

        /// <summary>
        /// Sauvegarde
        /// </summary>
        public void Save(DateTime effectiveOn, double amount)
        {
            Item.EffectiveOn = effectiveOn;
            Item.Amount = amount;
            Item.DateMaj = DateTime.Now;
            DatabaseAccess.Instance.Update(Item);
        }

        /// <summary>
        /// Suppression 
        /// </summary>
        public void Delete()
        {
            DatabaseAccess.Instance.Remove(Item);
        }
    }
}
