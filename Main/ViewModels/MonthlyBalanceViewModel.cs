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
        /// CanDelete 
        /// </summary>
        public double Balance => Item.Balance;

        public MonthlyBalanceViewModel(BankAccountBalance item) 
        {
            Item = item;
        }

        /// <summary>
        /// Suppression de la balance mensuelle du compte
        /// </summary>
        public void Delete()
        {
            Item.Delete();
        }

        /// <summary>
        /// Conversion en MonthlyRent -> MonthlyAppartmentViewModel
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static MonthlyBalanceViewModel From(BankAccountBalance item) => new MonthlyBalanceViewModel(item);

        /// <summary>
        /// Conversion en PeeBalance -> MonthlyPeeViewModel
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static List<MonthlyBalanceViewModel> From(IEnumerable<BankAccountBalance> items)
        {
            var result = new List<MonthlyBalanceViewModel>();
            foreach (var item in items)
            {
                result.Add(MonthlyBalanceViewModel.From(item));
            }
            return result;
        }
    }
}
