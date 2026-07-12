using static Business.BaseAccount;

namespace Business
{
    /// <summary>
    /// Définition des comptes bancaires, épargnes...
    /// </summary>
    public interface IBaseAccounts
    {
        public string Label { get; }

        public string AccountNo { get; }

        public AccountType Type { get; }
    }
}
