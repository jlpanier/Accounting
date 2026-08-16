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
        /// Toutes les actions
        /// </summary>
        public IEnumerable<ShareEntity> GetShares()
        {
            lock (dbLock)
            {
                return Db.Query<ShareEntity>("Select * from SHARES ");
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
        /// Balances mensuelles d'un compte bancaire
        /// </summary>
        public IEnumerable<AccountBalanceEntity> GetMonthlyBalances(int id, DateTime effectiveOn)
        {
            lock (dbLock)
            {
                return Db.Query<AccountBalanceEntity>("Select * from ACCOUNT_BALANCE WHERE BankAccountId = ? and EffectiveOn = ?", id, effectiveOn);
            }
        }

        /// <summary>
        /// Obtenir les transferts bancaires
        /// </summary>
        public IEnumerable<TranferEntity> GetTransfers(int id)
        {
            lock (dbLock)
            {
                return Db.Query<TranferEntity>("Select * from TRANSFER WHERE BankAccountId = ? ", id);
            }
        }

        /// <summary>
        /// Obtenir les ordres d'achat et de vente d'actions
        /// </summary>
        public IEnumerable<OrderEntity> GetOrders(int id)
        {
            lock (dbLock)
            {
                return Db.Query<OrderEntity>("Select * from ORDERS WHERE BankAccountId = ? ", id);
            }
        }

        /// <summary>
        /// Obtenir les ordres d'achat et de vente d'actions
        /// </summary>
        public IEnumerable<DividendeEntity> GetDividentes(int id)
        {
            lock (dbLock)
            {
                return Db.Query<DividendeEntity>("Select * from DIVIDENDES WHERE BankAccountId = ? ", id);
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

        /// <summary>
        /// Balances mensuelles d'un plan epargne entreprise
        /// </summary>
        public IEnumerable<PeaEntity> GetMonthlyPea(int id)
        {
            lock (dbLock)
            {
                return Db.Query<PeaEntity>("Select * from PEA WHERE BankAccountId = ?", id);
            }
        }

        /// <summary>
        /// Balances mensuelles d'un bien SCPI
        /// </summary>
        public IEnumerable<ScpiEntity> GetMonthlyScpi(int id)
        {
            lock (dbLock)
            {
                return Db.Query<ScpiEntity>("Select * from SCPI WHERE BankAccountId = ?", id);
            }
        }

        /// <summary>
        /// Prix de l'action  
        /// </summary>
        public IEnumerable<PriceShareEntity> GetPriceShareById(int id)
        {
            lock (dbLock)
            {
                return Db.Query<PriceShareEntity>("Select * from PRICESHARES WHERE ShareId = ?", id);
            }
        }

    }
}
