using Repository.Entities;

namespace Repository.Dbo
{
    public class DatabaseAccess: BaseDbo
    {
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
        private static readonly object _lock = new();

        public DatabaseAccess() : base()
        {
        }

        public AccountEntity? GetBankAccount(int bankNo)
        {
            lock (dbLock)
            {
                return Db.Query<AccountEntity>("Select * from ACCOUNT WHERE AccountNumero = ? ", bankNo).FirstOrDefault();
            }
        }

        public IEnumerable<AccountEntity> GetBankAccounts()
        {
            lock (dbLock)
            {
                return Db.Query<AccountEntity>("Select * from ACCOUNT ");
            }
        }
    }
}
