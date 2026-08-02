using static Business.BaseAccount;

namespace Business
{
    /// <summary>
    /// Définition des comptes bancaires, épargnes...
    /// </summary>
    public interface IBaseAccounts
    {
        /// <summary>
        /// Référence du compte bancaire
        /// </summary>
        int BankAccountId { get; }

        /// <summary>
        /// Label du compte bancaire
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Numéro du compte bancaire
        /// </summary>
        string AccountNo { get; }

        /// <summary>
        /// Date d'ouverture du compte bancaire
        /// </summary>
        DateTime StartOn { get; }

        /// <summary>
        /// Date de fermeture du compte bancaire
        /// </summary>
        DateTime EndOn { get; }

        /// <summary>
        /// Type du compte bancaire
        /// </summary>
        AccountType Type { get; }

        /// <summary>
        /// Sauvegarde du compte bancaire
        /// </summary>
        void Save(string accountNo, string label, DateTime dtStart, DateTime dtEnd, AccountType accountType);

        void Delete();
    }
}
