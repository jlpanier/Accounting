namespace Main.ViewModels
{
    public class SettingViewModel
    {
        /// <summary>
        /// Conversion 
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static List<SettingViewModel> From(List<Business.Setting> items)
        {
            var result = new List<SettingViewModel>();
            foreach (var item in items)
            {
                result.Add(SettingViewModel.From(item));
            }
            return result;
        }

        /// <summary>
        /// Conversion en Setting -> SettingViewModel
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static SettingViewModel From(Business.Setting item) => new SettingViewModel(item);

        private readonly Business.Setting Item;

        private SettingViewModel(Business.Setting item)
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
    }
}
