
using static Business.BaseAccount;

namespace Business
{
    /// <summary>
    /// Bilan global : épargne disponible, bloquée ou disponible à la retraite
    /// </summary>
    public class Overview
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

        public Overview(double disponible, double block, double retirement) 
        { 
            Disponible = disponible;
            Block = block;
            Retirement = retirement;
        }

        public string Label => "Overview";

        public string AccountNo => "Overview";

        public DateTime StartOn => DateTime.Now;

        public DateTime EndOn => DateTime.Now;

        public AccountType Type => AccountType.Overview;
    }
}
