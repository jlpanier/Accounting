namespace Business
{
    /// <summary>
    /// Ligne d'une transaction
    /// </summary>
    public class LineTransactions
    {
        /// <summary>
        /// Référence à la transaction
        /// </summary>
        public readonly ITransaction Transaction;

        /// <summary>
        /// Solde du compte à ce jour
        /// </summary>
        public double Solde { get; private set; }

        /// <summary>
        /// Date de la transaction
        /// </summary>
        public DateTime EffectiveOn => Transaction.EffectiveOn;

        /// <summary>
        /// Clef de la transaction
        /// </summary>
        public int Id => Transaction.Id;

        /// <summary>
        /// Libellé de la transaction
        /// </summary>
        public string Label => Transaction.Label;

        /// <summary>
        /// Montant de la transaction
        /// </summary>
        public double Amount => Transaction.Amount;

        public LineTransactions(ITransaction transaction, double solde) 
        {
            Transaction = transaction;
            Solde = solde;
        }
    }
}
