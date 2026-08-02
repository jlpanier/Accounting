using Repository.Dbo;
using Repository.Entities;

namespace Business
{
    public class PeaBalance
    {
        /// <summary>
        /// Création d'une nouvelle entrée 
        /// </summary>
        public static PeaBalance Create(int bankAccountId, DateTime effectiveOn, double cash, double investLibre, double amountLibre, double investProfile, double amountProfile, double unitPriceProfile)
        {
            var item = new PeaEntity
            {
                EffectiveOn = effectiveOn,
                BankAccountId = bankAccountId,
                Cash = cash,
                AmountLibre = amountLibre,
                AmountProfile = amountProfile,
                InvestLibre = investLibre,
                InvestProfile = investProfile,
                UnitPrice= unitPriceProfile,
                DateMaj = DateTime.Now
            };
            DatabaseAccess.Instance.Add(item);
            return new PeaBalance(item);
        }

        /// <summary>
        /// Référence vers l'entité de la base de données
        /// </summary>
        public readonly PeaEntity Item;

        /// <summary>
        /// Date d'effet du solde
        /// </summary>
        public DateTime EffectiveOn => Item.EffectiveOn;

        /// <summary>
        /// No du compte bancaire
        /// </summary>
        public int BankAccountId => Item.BankAccountId;

        /// <summary>
        /// Cash disponible sur ce PEA
        /// </summary>
        public double Cash => Item.Cash;

        /// <summary>
        /// Montant des actions géré de manière libre
        /// </summary>
        public double AmountLibre => Item.AmountLibre;

        /// <summary>
        /// Montant des actions gérées de manière profilé
        /// </summary>
        public double AmountProfile => Item.AmountProfile;

        /// <summary>
        /// Investissement dans les actions gérées de manière libre
        /// </summary>
        public double InvestLibre => Item.InvestLibre;

        /// <summary>
        /// Investissement dans les actions gérées de manière profilée
        /// </summary>
        public double InvestProfile => Item.InvestProfile;

        /// <summary>
        /// Prix unitaire de la gestion profilé
        /// </summary>
        public double UnitPrice => Item.UnitPrice;

        public PeaBalance(PeaEntity item)
        {
            Item = item;
        }

        /// <summary>
        /// Sauvegarde
        /// </summary>
        public int Save(DateTime effectiveOn, double cash, double investLibre, double amountLibre, double investProfile, double amountProfile, double unitPriceProfile)
        {
            Item.DateMaj = DateTime.Now;
            Item.EffectiveOn = effectiveOn;
            Item.Cash = cash;
            Item.DateMaj = DateTime.Now;
            Item.AmountLibre = amountLibre;
            Item.AmountProfile = amountProfile;
            Item.InvestLibre = investLibre;
            Item.InvestProfile = investProfile;
            Item.UnitPrice = unitPriceProfile;
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
