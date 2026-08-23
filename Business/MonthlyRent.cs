using Repository.Dbo;
using Repository.Entities;

namespace Business
{
    public class MonthlyRent
    {
        /// <summary>
        /// Création d'une nouvelle entrée du compte rendu mensuel
        /// </summary>
        public static MonthlyRent Create(int appartmentId, DateTime effectiveOn, string renter, double rent, double charges, double inout, double work, double exceptionel, double garantee, double gestion, double syndic, double transfer)
        {
            var item = new MonthlyRentEntity
            {
                EffectiveOn = effectiveOn,
                AppartmentId = appartmentId,
                Rent = rent,
                Provision=charges,
                InOut = inout,
                Work = work,
                Exceptionel = exceptionel,
                Garantee= garantee,
                Gestion = gestion,
                Renter = renter,
                Syndic= syndic,
                Transfer = transfer,
                DateMaj = DateTime.Now
            };
            DatabaseAccess.Instance.Add(item);
            return new MonthlyRent(item);
        }

        /// <summary>
        /// Référence vers l'entité de la base de données
        /// </summary>
        public readonly MonthlyRentEntity Item;

        /// <summary>
        /// Date d'effet du compte rendu mensuel
        /// </summary>
        public DateTime EffectiveOn => Item.EffectiveOn;

        /// <summary>
        /// No du compte bancaire
        /// </summary>
        public int BankAccountId => Item.AppartmentId;

        /// <summary>
        /// Locatraire du logement
        /// </summary>
        public string Renter => Item.Renter;

        /// <summary>
        /// Loyer reçu pour le mois
        /// </summary>
        public double Rent => Item.Rent;

        /// <summary>
        /// Charge pour le mois
        /// </summary>
        public double Provision => Item.Provision;

        /// <summary>
        /// Frais entrée / départ d'un nouveau locataire
        /// </summary>
        public double InOut => Item.InOut;

        /// <summary>
        /// Frais de travaux pour le mois
        /// </summary>
        public double Work => Item.Work;

        /// <summary>
        /// Frais exceptionel pour le mois
        /// </summary>
        public double Exceptionel => Item.Exceptionel;

        /// <summary>
        /// Frais de garantie pour le mois
        /// </summary>
        public double Garantee => Item.Garantee;

        /// <summary>
        /// Frais de gestion pour le mois
        /// </summary>
        public double Gestion => Item.Gestion;

        /// <summary>
        /// Frais syndic pour le mois
        /// </summary>
        public double Syndic => Item.Syndic;

        /// <summary>
        /// Payment/Transfer de loyer pour le mois
        /// </summary>
        public double Transfer => Item.Transfer;

        public MonthlyRent(MonthlyRentEntity item)
        {
            Item = item;
        }

        /// <summary>
        /// Sauvegarde
        /// </summary>
        public int Save(DateTime effectiveOn, string renter, double rent, double charges, double inout, double work, double exceptionel, double garantee, double gestion, double syndic, double transfer)
        {
            Item.Renter = renter;
            Item.EffectiveOn = effectiveOn;
            Item.Rent = rent;
            Item.Provision = charges;
            Item.InOut = inout;
            Item.Work = work;
            Item.Exceptionel = exceptionel;
            Item.Garantee = garantee;
            Item.Gestion = gestion;
            Item.Syndic = syndic;
            Item.Transfer = transfer;
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
