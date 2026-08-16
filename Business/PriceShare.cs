using Repository.Dbo;
using Repository.Entities;

namespace Business
{
    public class PriceShare
    {
        /// <summary>
        /// Création d'une nouvelle entrée 
        /// </summary>
        public static PriceShare Create(int shareId, DateTime effectiveOn, double unitprice)
        {
            var item = new PriceShareEntity
            {
                EffectiveOn = effectiveOn,
                UnitPrice=unitprice,
                ShareId=shareId,
                DateMaj = DateTime.Now
            };
            DatabaseAccess.Instance.Add(item);
            return new PriceShare(item);
        }

        /// <summary>
        /// Référence vers l'entité de la base de données
        /// </summary>
        public readonly PriceShareEntity Item;

        /// <summary>
        /// Référence
        /// </summary>
        public int Id => Item.Id;

        /// <summary>
        /// Date d'effet du solde
        /// </summary>
        public DateTime EffectiveOn => Item.EffectiveOn;

        /// <summary>
        /// Prix unitaire de l'action
        /// </summary>
        public double UnitPrice => Item.UnitPrice;

        public PriceShare(PriceShareEntity item)
        {
            Item = item;
        }

        /// <summary>
        /// Sauvegarde
        /// </summary>
        public int Save(DateTime effectiveOn, double unitprice)
        {
            Item.DateMaj = DateTime.Now;
            Item.UnitPrice = unitprice;
            Item.EffectiveOn = effectiveOn;
            int rows = DatabaseAccess.Instance.Update(Item);
            return rows;
        }

        /// <summary>
        /// Suppression de la balance mensuelle 
        /// </summary>
        public void Delete()
        {
            DatabaseAccess.Instance.Remove(Item);
        }
    }

}
