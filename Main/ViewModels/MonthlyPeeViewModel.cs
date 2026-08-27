using Business;

namespace Main.ViewModels
{
    public class MonthlyPeeViewModel: BaseViewModel
    {
        #region Propriétés

        /// <summary>
        /// Date 
        /// </summary>
        public DateTime EffectiveOn
        {
            get => _effectiveOn;
            set
            {
                if (_effectiveOn != value)
                {
                    _effectiveOn = value;
                    NotifyPropertyChanged(nameof(EffectiveOn));
                }
            }
        }
        public DateTime _effectiveOn;

        /// <summary>
        /// Montant disponible
        /// </summary>
        public double Disponible
        {
            get => _disponible;
            set
            {
                if (_disponible != value)
                {
                    _disponible = value;
                    NotifyPropertyChanged(nameof(Disponible));
                }
            }
        }
        public double _disponible = 0;

        /// <summary>
        /// Montant blocké
        /// </summary>
        public double Blocked
        {
            get => _blocked;
            set
            {
                if (_blocked != value)
                {
                    _blocked = value;
                    NotifyPropertyChanged(nameof(Blocked));
                }
            }
        }
        public double _blocked = 0.0;

        /// <summary>
        /// Montant blocké jusqu'à la retraite
        /// </summary>
        public double Retirement
        {
            get => _retirement;
            set
            {
                if (_retirement != value)
                {
                    _retirement = value;
                    NotifyPropertyChanged(nameof(Retirement));
                }
            }
        }
        public double _retirement = 0.0;

        /// <summary>
        /// Montant total
        /// </summary>
        public double Amount => Disponible + Blocked + Retirement;

        /// <summary>
        /// Référence du compte bancaire de l'appartement
        /// </summary>
        public int BankAccountId;

        #endregion

        /// <summary>
        /// Conversion en MonthlyRent -> MonthlyAppartmentViewModel
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static MonthlyPeeViewModel From(PeeBalance item) => new MonthlyPeeViewModel()
        {
            BankAccountId = item.BankAccountId,
            Retirement = item.Retirement,
            EffectiveOn = item.EffectiveOn,
            Blocked = item.Blocked,
            Disponible = item.Disponible,
        };

        /// <summary>
        /// Conversion en PeeBalance -> MonthlyPeeViewModel
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static List<MonthlyPeeViewModel> From(IEnumerable<PeeBalance> items)
        {
            var result = new List<MonthlyPeeViewModel>();
            foreach (var item in items)
            {
                result.Add(MonthlyPeeViewModel.From(item));
            }
            return result;
        }
    }
}
