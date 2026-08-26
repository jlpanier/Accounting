using Repository.Dbo;
using Repository.Entities;
using static Business.Share;

namespace Business
{
    /// <summary>
    /// Gestion des transactions de virement
    /// </summary>
    public class Order: ITransaction
    {
        public enum OrderType { Buy, Sell }
        
        /// <summary>
        /// Création d'une transaction de virement
        /// </summary>
        public static Order Create(int shareId, int bankAccountId, DateTime effectiveOn, double quantity, double unitprice, double fees, double tax)
        {
            var item = new OrderEntity()
            {
                ShareId = shareId,
                BankAccountId = bankAccountId,
                EffectiveOn = effectiveOn,
                Quantity=quantity,
                UnitPrice = unitprice,
                Fees = fees,
                Tax = tax,
                DateMaj = DateTime.Now,
            };
            DatabaseAccess.Instance.Add(item);
            return new Order(item);
        }

        /// <summary>
        /// Référence vers l'entité de la base de données
        /// </summary>
        public readonly OrderEntity Item;

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
        public int ShareId => Item.ShareId;

        /// <summary>
        /// Référence du compte bancaire
        /// </summary>
        public int BankAccountId => Item.BankAccountId;

        /// <summary>
        /// Nombre de part de la transaction
        /// </summary>
        public double Quantity => Item.Quantity;

        /// <summary>
        /// Prix unitaire de la part
        /// </summary>
        public double UnitPrice => Item.UnitPrice;

        /// <summary>
        /// Montant de la commission
        /// </summary>
        public double Fees => Item.Fees;

        /// <summary>
        /// Montant des taxes
        /// </summary>
        public double Tax => Item.Tax;

        /// <summary>
        /// Montant total
        /// </summary>
        public double Amount => -UnitPrice* Quantity - Tax - Fees;

        /// <summary>
        /// Type d'opération: achat ou vente
        /// </summary>
        public OrderType Type => Quantity > 0 ? OrderType.Buy : OrderType.Sell;

        /// <summary>
        /// Action liée à l'opération
        /// </summary>
        public Share? Share => Business.Share.All.FirstOrDefault(_=>_.Id==ShareId);

        /// <summary>
        /// Libellé de la transaction
        /// </summary>
        public string Label
        {
            get
            {
                var result = string.Empty;
                if (Share!=null)
                {
                    if (Quantity > 0)
                    {
                        if (Share.Type == TypeShare.Profile)
                        {
                            result = $"Achat de {Quantity:N4} parts de {Share.Name} au prix unitaire de {UnitPrice:N4} €";
                        }
                        else
                        {
                            result = $"Achat de {Quantity:N0} parts de {Share.Name} au prix unitaire de {UnitPrice:N2} € comprenant {Fees:N2} € de commission et {Tax:N2} € de frais";
                        }
                    }
                    else
                    {
                        if (Share.Type == TypeShare.Profile)
                        {
                            result = $"Vente de {-Quantity:N4} parts de {Share.Name} au prix unitaire de {UnitPrice:N4} € ";
                        }
                        else
                        {
                            result = $"Vente de {-Quantity:N0} parts de {Share.Name} au prix unitaire de {UnitPrice:N2} € comprenant {Fees:N2} € de commission et {Tax:N2} € de frais";
                        }
                    }
                }
                return result;
            }
        }

        public Order(OrderEntity item)
        {
            Item = item;
        }

        /// <summary>
        /// Sauvegarde
        /// </summary>
        public void Save(DateTime effectiveOn, double quantity, double unitPrice, double fees, double tax)
        {
            Item.EffectiveOn = effectiveOn;
            Item.Quantity = quantity;
            Item.UnitPrice = unitPrice;
            Item.Fees = fees;
            Item.Tax = tax;
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
