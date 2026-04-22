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

        public int PurgeSolutions(int columns)
        {
            lock (dbLock)
            {
                return Db.Execute("DELETE FROM SOLUTIONS WHERE Columns = ?", columns);
            }
        }

        public IEnumerable<SolutionEntity> Get(int columns)
        {
            lock (dbLock)
            {
                return Db.Query<SolutionEntity>("Select * from SOLUTIONS WHERE Columns = ? ", columns);
            }
        }
    }
}
