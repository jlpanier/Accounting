using Repository.Dbo;

namespace Business
{
    /// <summary>
    /// Gestion de la configuration
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Instance
        /// </summary>
        public static Settings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Settings();
                }
                return _instance;
            }
        }
        private static Settings? _instance;

        private Settings()
        {
        }

        /// <summary>
        /// Chargement de la configuration
        /// </summary>
        public List<Setting> All
        {
            get
            {
                if (_all == null)
                {
                    _all = new List<Setting>();
                    DatabaseAccess.Instance.GetSettings().ToList().ForEach(_ => _all.Add(Setting.From(_)));
                }
                return _all;
            }
        }
        private List<Setting>? _all;

        /// <summary>
        /// Création d'une nouvelle entité
        /// </summary>
        public Setting Add(string key, string val, string descr)
        {
            var item = All.FirstOrDefault(_=>_.Key==key);
            if (item == null) 
            {
                item = Setting.Create(key, val, descr);
                All.Add(item);
            }
            else
            {
                item.Save(key, val, descr);
            }
            return item;
        }

        /// <summary>
        /// Obtenir la configuration d'une string
        /// </summary>
        private string GetString(string key, string defaultvalue)
        {
            var setting = All.FirstOrDefault(_=>_.Key == key);
            return setting == null ? defaultvalue : setting.Val;
        }

        /// <summary>
        /// Obtenir la configuration d'un entier
        /// </summary>
        private int GetInt(string key, int defaultvalue)
        {
            int result = defaultvalue;
            var setting = All.FirstOrDefault(_ => _.Key == key);
            if (setting != null)
            {
                if (int.TryParse(setting.Val, out int val))
                {
                    result = val;
                }
            }
            return result;
        }

        /// <summary>
        /// Obtenir la configuration d'un double
        /// </summary>
        private double GetDouble(string key, double defaultvalue)
        {
            double result = defaultvalue;
            var setting = All.FirstOrDefault(_ => _.Key == key);
            if (setting != null)
            {
                if (double.TryParse(setting.Val, out double val))
                {
                    result = val;
                }
            }
            return result;
        }

        /// <summary>
        /// Loyer de l'appartement par defaut lors d'une nouvelle saisie
        /// </summary>
        public double Rent => GetDouble("appartement.rent.default", 339.26);

        /// <summary>
        /// Provision de l'appartement par defaut lors d'une nouvelle saisie
        /// </summary>
        public double Provision => GetDouble("appartement.provision.default", 174.0);

        /// <summary>
        /// Frais de garantie de l'appartement par defaut lors d'une nouvelle saisie
        /// </summary>
        public double Garanty => GetDouble("appartement.garanty.default", 15.40);

        /// <summary>
        /// Commission de l'appartement par defaut lors d'une nouvelle saisie
        /// </summary>
        public double Gestion => GetDouble("appartement.gestion.default", 15.40);

        /// <summary>
        /// Nom du locataire de l'appartement par defaut lors d'une nouvelle saisie
        /// </summary>
        public string Renter => GetString("appartement.renter.default", "Angelina Ducarteron");
    }
}
