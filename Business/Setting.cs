using Repository.Dbo;
using Repository.Entities;

namespace Business
{
    /// <summary>
    /// Gestion d'une" configuration
    /// </summary>
    public class Setting
    {
        /// <summary>
        /// Création d'une entité
        /// </summary>
        public static Setting Create(string key, string val, string descr)
        {
            var item = new SettingsEntity
            {
                Key = key,
                Val = val,
                Desc = descr,
                DateMaj = DateTime.Now,
            };
            DatabaseAccess.Instance.Add(item);
            return new Setting(item);
        }

        /// <summary>
        /// Conversion de l'entité en configuration
        /// </summary>
        public static Setting From(SettingsEntity item) => new Setting(item);

        public readonly SettingsEntity Item;

        private Setting(SettingsEntity item)
        {
            Item = item;
        }

        /// <summary>
        /// Référence de la configuration
        /// </summary>
        public string Key => Item.Key;

        /// <summary>
        /// Valeur de la configuration
        /// </summary>
        public string Val => Item.Val;

        /// <summary>
        /// Description de la configuration
        /// </summary>
        public string Desc => Item.Desc;

        /// <summary>
        /// Sauvegarde
        /// </summary>
        public int Save(string key, string val, string descr)
        {
            Item.DateMaj = DateTime.Now;
            Item.Key = key;
            Item.Desc = descr;
            Item.Val = val;
            return DatabaseAccess.Instance.Update(Item);
        }
    }
}