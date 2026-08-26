using static Business.Share;

namespace Business
{
    /// <summary>
    /// Comptabiliser les actions du mois
    /// </summary>
    public class MonthlyShare 
    {
        /// <summary>
        /// Référence de l'action
        /// </summary>
        public readonly Share Item;

        /// <summary>
        /// Liste des ordres sur cette action
        /// </summary>
        public readonly List<Order> Orders;

        /// <summary>
        /// Mois en cours
        /// </summary>
        public readonly DateTime EffectiveOn;

        /// <summary>
        /// Référence de l'action
        /// </summary>
        public int ShareId => Item.Id;

        /// <summary>
        /// Type d'action
        /// </summary>
        public TypeShare Type => Item.Type;

        /// <summary>
        /// Code de l'action
        /// </summary>
        public string Code => Item.Code;

        /// <summary>
        /// Nom de l'action
        /// </summary>
        public string Label => Item.Label;

        /// <summary>
        /// Nombre de parts
        /// </summary>
        public double Quantity
        {
            get
            {
                double quantity = 0;
                foreach (var order in Orders)
                {
                    quantity += order.Quantity;
                }
                return quantity;
            }
        }

        /// <summary>
        /// Prix unitaire de la part
        /// </summary>
        public double UnitPrice { get; set; }

        /// <summary>
        /// Valorisation de l'action
        /// </summary>
        public double Amount => Quantity * UnitPrice;

        /// <summary>
        /// Nombre de parts
        /// </summary>
        public double Investement
        {
            get
            {
                double invetment = 0;
                foreach (var order in Orders)
                {
                    invetment += order.Amount;
                }
                return invetment;
            }
        }

        /// <summary>
        /// Gain ou perte sur l'action
        /// </summary>
        public double GainLoss => Amount + Investement;

        /// <summary>
        /// Pourcentage de Gain
        /// </summary>
        public double PercentageGainLoss => Investement == 0 ? 0 : 100 * (Amount+ Investement) / Investement;

        /// <summary>
        /// Gain ou perte sur l'action en clair
        /// </summary>
        public string GainLossLabel => GainLoss >= 0 ? $"\u2197 {GainLoss:N2} € ({PercentageGainLoss:N0}%)" : $"\u2198 {GainLoss:N2} € ({PercentageGainLoss:N0}%)";

        /// <summary>
        /// Référence au prix en cours de l'action
        /// </summary>
        public readonly PriceShare? Price;

        /// <summary>
        /// Référence au prix en cours de l'action
        /// </summary>
        public int PriceShareId {  get; }

        public MonthlyShare(int shareId, DateTime effectiveOn, Order order)
        {
            Item = Share.All.First(s => s.Id == shareId);
            Price = Item.Amounts.FirstOrDefault(_=>_.EffectiveOn== effectiveOn);
            UnitPrice = Price?.UnitPrice ?? 0;
            PriceShareId = Price?.Id ?? 0;
            Orders = new List<Order>() { order };
        }

        /// <summary>
        /// Ajout d'un nouvel ordre achat/vente
        /// </summary>
        public void Add(Order order)
        {
            Orders.Add(order);
        }
    }
}
