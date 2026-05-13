using Repository.Entities;
using SQLite;


namespace Repository.Dbo
{
    /// <summary>
    /// Gestion de la base de données SQLite
    /// </summary>
    public abstract class BaseDbo:IDisposable
    {
        /// <summary>
        /// Nom de la base de données SQLite
        /// </summary>
        public const string DatabaseName = "SQUARE.sqlite";

        /// <summary>
        /// Lock pour les accès à la base de données
        /// </summary>
        protected static readonly object dbLock = new object();

        /// <summary>
        /// Instance de la connexion à la base de données SQLite
        /// </summary>
        private SQLiteConnection? _db = null;

        public BaseDbo()
        {
        }

        /// <summary>
        /// Instance de la connexion à la base de données SQLite
        /// </summary>
        public SQLiteConnection Db
        {
            get
            {
                if (_db == null)
                {
                    _db = new SQLiteConnection(DbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex, false);
                }
                return _db;
            }
        }

        /// <summary>
        /// Chemin de la base de données
        /// </summary>
        public static string DbPath { get; private set; } = string.Empty;

        /// <summary>
        /// VRAI, si le fichier de la base de données existe
        /// </summary>
        /// <returns></returns>
        public bool IsReady() => !string.IsNullOrEmpty(DbPath) && File.Exists(DbPath);

        /// <summary>
        /// Initialisation
        /// </summary>
        /// <param name="databasePath"></param>
        /// <param name="busyTimeout"></param>
        public void Init(string databasePath, double busyTimeout = 30)
        {
            DbPath = databasePath;
            if (!File.Exists(DbPath)) throw new FileNotFoundException("File does not exists", DbPath);
            Db.BusyTimeout = TimeSpan.FromSeconds(busyTimeout);
            //DropTable<SolutionEntity>();
            CreateTable<AccountEntity>();
            CreateTable<AccountBalanceEntity>();
        }

        /// <summary>
        /// Is this instance disposed?
        /// </summary>
        protected bool Disposed { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            Close();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose worker method. See http://coding.abel.nu/2012/01/disposable
        /// </summary>
        /// <param name="disposing">Are we disposing? 
        /// Otherwise we're finalizing.</param>
        protected virtual void Dispose(bool disposing)
        {
            Disposed = true;
        }

        /// <summary>
        /// Fermeture de la connexion à la base de données SQLite
        /// </summary>
        public void Close()
        {
            if (_db != null)
            {
                _db.Close();
                _db = null;
            }
        }

        /// <summary>
        /// Mise à jour d'une entité dans la base de données SQLite
        /// </summary>
        public void Update(BaseEntity entity)
        {
            Db.InsertOrReplace(entity);
        }

        /// <summary>
        /// Ajout d'une entité dans la base de données SQLite
        /// </summary>
        public void Add(BaseEntity entity)
        {
            Db.Insert(entity);
        }

        /// <summary>
        /// Mise à jour d'une entité dans la base de données SQLite
        /// </summary>
        public void Save(IEnumerable<BaseEntity> entities)
        {
            lock (dbLock)
            {
                Db.RunInTransaction(() =>
                {
                    foreach (var e in entities)
                    {
                        e.Save(Db);
                    }
                });
            }
        }

        /// <summary>
        /// Suppression d'une entité dans la base de données SQLite
        /// </summary>
        public void Remove(params BaseEntity[] entities)
        {
            Remove((IEnumerable<BaseEntity>)entities);
        }

        /// <summary>
        /// Suppression de plusierus entités dans la base de données SQLite
        /// </summary>
        public void Remove(IEnumerable<BaseEntity> entities)
        {
            lock (dbLock)
            {
                Db.RunInTransaction(() =>
                {
                    foreach (var e in entities)
                    {
                        e.Remove(Db);
                    }
                });
            }
        }

        /// <summary>
        /// Ajout d'une colonne dans une table de la base de données SQLite
        /// </summary>
        public int AddColumn(string tableName, string columnName, string type, string lenght)
        {
            lock (dbLock)
            {
                try
                {
                    return Db.Execute($"alter table {tableName} add column {columnName} {type} ({lenght})");
                }
                catch
                {
                    // Nothing
                }
                return -1;
            }
        }

        /// <summary>
        /// Ajout d'une colonne dans une table de la base de données SQLite
        /// </summary>
        public int AddColumn(string tableName, string columnName, string type)
        {
            lock (dbLock)
            {
                try
                {
                    return Db.Execute($"alter table {tableName} add column {columnName} {type}");
                }
                catch
                {
                    // Nothing
                }
                return -1;
            }
        }

        /// <summary>
        /// Execution dans une table de la base de données SQLite
        /// </summary>
        public T ExecuteScalar<T>(string query, params object[] args)
        {
            lock (Db)
            {
                return Db.ExecuteScalar<T>(query, args);
            }
        }

        public int Execute(string query, params object[] args)
        {
            lock (Db)
            {
                return Db.Execute(query, args);
            }
        }

        public int Insert(object obj)
        {
            lock (Db)
            {
                return Db.Insert(obj);
            }
        }

        public int Update(object obj)
        {
            lock (Db)
            {
                return Db.Update(obj);
            }
        }

        public int Delete(object objectToDelete)
        {
            lock (Db)
            {
                return Db.Delete(objectToDelete);
            }
        }

        public int Delete<T>(object primaryKey)
        {
            lock (Db)
            {
                return Db.Delete<T>(primaryKey);
            }
        }

        public int DeleteAll<T>()
        {
            lock (Db)
            {
                return Db.DeleteAll<T>();
            }
        }

        public void CreateTable<T>() where T : class
        {
            lock (Db)
            {
                Db.CreateTable<T>();
            }
        }

        public void DropTable<T>() where T : class
        {
            lock (Db)
            {
                Db.DropTable<T>();
            }
        }

    }

}
