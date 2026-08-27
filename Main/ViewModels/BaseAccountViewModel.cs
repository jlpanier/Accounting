namespace Main.ViewModels
{
    public abstract partial class BaseAccountViewModel: BaseViewModel
    {
        #region Propriétés

        /// <summary>
        /// Référence de l'appartement
        /// </summary>
        public string Titel
        {
            get => _titel;
            set
            {
                if (_titel != value)
                {
                    _titel = value;
                    NotifyPropertyChanged(nameof(Titel));
                }
            }
        }
        public string _titel = "";

        /// <summary>
        /// Référence de l'appartement
        /// </summary>
        public string SubTitel
        {
            get => _subtitel;
            set
            {
                if (_subtitel != value)
                {
                    _subtitel = value;
                    NotifyPropertyChanged(nameof(SubTitel));
                }
            }
        }
        public string _subtitel = "";

        /// <summary>
        /// Référence du compte bancaire de l'appartement
        /// </summary>
        public int BankAccountId;

        #endregion

        protected void Init(int bankAccountId, string titel, string subtitle)
        {
            Titel = titel;
            SubTitel = subtitle;
            BankAccountId = bankAccountId;
        }
    }
}
