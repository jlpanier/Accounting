
namespace Business
{
    /// <summary>
    /// Bilan global : épargne disponible, bloquée ou disponible à la retraite
    /// </summary>
    public class OverviewAccounts: IBaseAccounts
    {
        /// <summary>
        /// Epargne disponible
        /// </summary>
        public readonly double Disponible;

        /// <summary>
        /// Epargne bloquée
        /// </summary>
        public readonly double Block;

        /// <summary>
        /// Epargne disponible à la retraite
        /// </summary>
        public readonly double Retirement;

        public OverviewAccounts(double disponible, double block, double retirement) 
        { 
            Disponible = disponible;
            Block = block;
            Retirement = retirement;
        }
    }
}
