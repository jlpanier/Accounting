
namespace Business
{
    /// <summary>
    /// Définition des transactions...
    /// </summary>
    public interface ITransaction
    {
        /// <summary>
        /// Référence de la transaction existante
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Référence du compte bancaire
        /// </summary>
        int BankAccountId { get; }

        /// <summary>
        /// Description de la transaction
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Montant de la transaction
        /// </summary>
        double Amount { get; }

        /// <summary>
        /// Date de la transaction
        /// </summary>
        DateTime EffectiveOn { get; }

        /// <summary>
        /// Suppression de la transaction
        /// </summary>
        void Delete();
    }

}
