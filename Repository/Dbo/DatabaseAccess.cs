using Repository.Entities;

namespace Repository.Dbo
{
    /// <summary>
    /// Gestion de la base de données SQLite
    /// </summary>
    public class DatabaseAccess: BaseDbo
    {
        /// <summary>
        /// Instance de la base de données SQLite
        /// </summary>
        public static DatabaseAccess Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null) _instance = new DatabaseAccess();
                    }
                }
                return _instance;
            }
        }
        private static DatabaseAccess? _instance;

        /// <summary>
        /// Lock
        /// </summary>
        private static readonly object _lock = new();

        public DatabaseAccess() : base()
        {
        }

        /// <summary>
        /// Compte bancaire lié 
        /// </summary>
        public AccountEntity? GetBankAccountNo(string bankNo)
        {
            lock (dbLock)
            {
                return Db.Query<AccountEntity>("Select * from ACCOUNT WHERE AccountNo = ? ", bankNo).FirstOrDefault();
            }
        }

        /// <summary>
        /// Compte bancaire lié 
        /// </summary>
        public AccountEntity? GetBankAccountId(int bankId)
        {
            lock (dbLock)
            {
                return Db.Query<AccountEntity>("Select * from ACCOUNT WHERE Id = ? ", bankId).FirstOrDefault();
            }
        }

        /// <summary>
        /// Tous les comptes bancaires  
        /// </summary>
        public IEnumerable<AccountEntity> GetAccounts()
        {
            lock (dbLock)
            {
                return Db.Query<AccountEntity>("Select * from ACCOUNT ");
            }
        }

        /// <summary>
        /// Balances mensuelles d'un compte bancaire
        /// </summary>
        public IEnumerable<AccountBalanceEntity> GetMonthlyBalances(int id)
        {
            lock (dbLock)
            {
                return Db.Query<AccountBalanceEntity>("Select * from ACCOUNT_BALANCE WHERE BankAccountId = ?", id);
            }
        }

        /// <summary>
        /// Balances mensuelles d'un plan epargne entreprise
        /// </summary>
        public IEnumerable<PeeEntity> GetMonthlyPee(int id)
        {
            lock (dbLock)
            {
                return Db.Query<PeeEntity>("Select * from PEE WHERE BankAccountId = ?", id);
            }
        }
    }
}
