using Repository.Dbo;
using Repository.Entities;

namespace Business
{
    public class ScpiBalance
    {
        /// <summary>
        /// Création d'une nouvelle entrée du solde
        /// </summary>
        public static ScpiBalance Create(int bankAccountId, DateTime effectiveOn, int numberOfShares=0, double unitPrice=0.0, double rent=0.0)
        {
            var item = new ScpiEntity
            {
                EffectiveOn = effectiveOn,
                BankAccountId = bankAccountId,
                NumberOfShares = numberOfShares,
                Rent = rent,
                UnitPrice = unitPrice,
                DateMaj = DateTime.Now
            };
            DatabaseAccess.Instance.Add(item);
            return new ScpiBalance(item);
        }

        /// <summary>
        /// Référence vers l'entité de la base de données
        /// </summary>
        public readonly ScpiEntity Item;

        /// <summary>
        /// Date d'effet du solde
        /// </summary>
        public DateTime EffectiveOn => Item.EffectiveOn;

        /// <summary>
        /// No du compte bancaire
        /// </summary>
        public int BankAccountId => Item.BankAccountId;

        /// <summary>
        /// Nombre de parts détenues
        /// </summary>
        public int NumberOfShares => Item.NumberOfShares;

        /// <summary>
        /// Prix unitaire de la part 
        /// </summary>
        public double UnitPrice => Item.UnitPrice;

        /// <summary>
        /// Nombre de parts détenues
        /// </summary>
        public double TotalPrice => NumberOfShares * UnitPrice;

        /// <summary>
        /// Loyer mensuel perçu
        /// </summary>
        public double Rent => Item.Rent;

        public ScpiBalance(ScpiEntity item)
        {
            Item = item;
        }

        /// <summary>
        /// Sauvegarde
        /// </summary>
        public int Save(DateTime effectiveOn, int numberOfShares, double unitPrice, double rent)
        {
            Item.DateMaj = DateTime.Now;
            Item.EffectiveOn = effectiveOn;
            Item.NumberOfShares = numberOfShares;
            Item.UnitPrice = unitPrice;
            Item.Rent = rent;
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
