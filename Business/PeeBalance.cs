using Repository.Dbo;
using Repository.Entities;

namespace Business
{
    public class PeeBalance
    {
        /// <summary>
        /// Création d'une nouvelle entrée du solde
        /// </summary>
        public static void Create(int bankAccountId, DateTime effectiveOn, double disponible, double retirement, double blocked)
        {
            var item = new PeeEntity
            {
                EffectiveOn = effectiveOn,
                BankAccountId = bankAccountId,
                Disponible = disponible,
                Blocked = blocked,
                Retirement = retirement,
                DateMaj = DateTime.Now
            };
            DatabaseAccess.Instance.Add(item);
        }

        /// <summary>
        /// Référence vers l'entité de la base de données
        /// </summary>
        public readonly PeeEntity Item;

        /// <summary>
        /// Date d'effet du solde
        /// </summary>
        public DateTime EffectiveOn => Item.EffectiveOn;

        /// <summary>
        /// No du compte bancaire
        /// </summary>
        public int BankAccountId => Item.BankAccountId;

        /// <summary>
        /// Cash disponible sur ce plan épargne entreprise (PEE)
        /// </summary>
        public double Disponible => Item.Disponible;

        /// <summary>
        /// Cash disponible à la retraite sur ce plan épargne entreprise (PEE)
        /// </summary>
        public double Retirement => Item.Retirement;

        /// <summary>
        /// Cash bloqué sur ce plan épargne entreprise (PEE)
        /// </summary>
        public double Blocked => Item.Blocked;

        public PeeBalance(PeeEntity item)
        {
            Item = item;
        }

        /// <summary>
        /// Sauvegarde
        /// </summary>
        public int Save(double disponible, double retirement, double blocked)
        {
            Item.DateMaj = DateTime.Now;
            Item.Disponible = disponible;
            Item.Retirement = retirement;
            Item.Blocked = blocked;
            Item.DateMaj = DateTime.Now;
            int rows = DatabaseAccess.Instance.Update(Item);
            return rows;
        }

        /// <summary>
        /// Suppression de la balance mensuelle du compte
        /// </summary>
        public void Delete()
        {
            DatabaseAccess.Instance.Remove(Item);
        }
    }
}
