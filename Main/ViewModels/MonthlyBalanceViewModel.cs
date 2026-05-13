using Business;

namespace Main.ViewModels
{
    /// <summary>
    /// Une ligne de la balance du compte 
    /// </summary>
    public class MonthlyBalanceViewModel
    {
        /// <summary>
        /// Une ligne de la balance du compte 
        /// </summary>
        public readonly BankAccountBalance Item;

        /// <summary>
        /// Date de la balance - début du mois mois
        /// </summary>
        public DateTime EffectiveOn => Item.EffectiveOn;

        /// <summary>
        /// Affichage de la date de la balance - mois et année
        /// </summary>
        public string Month => Item.EffectiveOn.ToString("MMMM yyyy");

        /// <summary>
        /// Balance 
        /// </summary>
        public double Balance => Item.Balance;

        public MonthlyBalanceViewModel(BankAccountBalance item) 
        {
            Item = item;
        }
    }
}
