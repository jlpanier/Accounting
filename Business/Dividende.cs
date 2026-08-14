using Repository.Dbo;
using Repository.Entities;

namespace Business
{
    /// <summary>
    /// Gestion des transactions de virement
    /// </summary>
    public class Dividende: ITransaction
    {
        /// <summary>
        /// Création d'une transaction de virement
        /// </summary>
        public static Dividende Create(int shareId, int bankAccountId, DateTime effectiveOn, double amount)
        {
            var item = new DividendeEntity()
            {
                ShareId = shareId,
                BankAccountId = bankAccountId,
                EffectiveOn = effectiveOn,
                Amount=amount,
                DateMaj = DateTime.Now,
            };
            DatabaseAccess.Instance.Add(item);
            return new Dividende(item);
        }

        /// <summary>
        /// Référence vers l'entité de la base de données
        /// </summary>
        public readonly DividendeEntity Item;

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
        public double Amount => Item.Amount;

        /// <summary>
        /// Action liée à l'opération
        /// </summary>
        public Share? Share => Business.Share.All.FirstOrDefault(_ => _.Id == ShareId);

        /// <summary>
        /// Libellé de la transaction
        /// </summary>
        public string Label
        {
            get
            {
                var result = string.Empty;
                if (Share != null)
                {
                    result = $"Dividende de {Share.Name}";
                }
                return result;
            }
        }

        public Dividende(DividendeEntity item)
        {
            Item = item;
        }

        /// <summary>
        /// Sauvegarde
        /// </summary>
        public void Save(int shareId, DateTime effectiveOn, double amount)
        {
            Item.EffectiveOn = effectiveOn;
            Item.Amount = amount;
            Item.DateMaj = DateTime.Now;
            Item.ShareId = shareId;
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
