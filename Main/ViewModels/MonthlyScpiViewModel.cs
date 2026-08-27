using Business;

namespace Main.ViewModels
{
    public class MonthlyScpiViewModel : BaseViewModel
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
        public double UnitPrice
        {
            get => _unitPrice;
            set
            {
                if (_unitPrice != value)
                {
                    _unitPrice = value;
                    NotifyPropertyChanged(nameof(UnitPrice));
                }
            }
        }
        public double _unitPrice = 0;

        /// <summary>
        /// Montant blocké
        /// </summary>
        public double NumberOfShares
        {
            get => _numberOfShares;
            set
            {
                if (_numberOfShares != value)
                {
                    _numberOfShares = value;
                    NotifyPropertyChanged(nameof(NumberOfShares));
                }
            }
        }
        public double _numberOfShares = 0.0;

        /// <summary>
        /// Montant blocké jusqu'à la retraite
        /// </summary>
        public double Rent
        {
            get => _rent;
            set
            {
                if (_rent != value)
                {
                    _rent = value;
                    NotifyPropertyChanged(nameof(Rent));
                }
            }
        }
        public double _rent = 0.0;

        /// <summary>
        /// Montant total
        /// </summary>
        public double Amount => UnitPrice * NumberOfShares;

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
        public static MonthlyScpiViewModel From(ScpiBalance item) => new MonthlyScpiViewModel()
        {
            BankAccountId = item.BankAccountId,
            Rent = item.Rent,
            EffectiveOn = item.EffectiveOn,
            NumberOfShares = item.NumberOfShares,
            UnitPrice = item.UnitPrice,
        };

        /// <summary>
        /// Conversion en PeeBalance -> MonthlyPeeViewModel
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static List<MonthlyScpiViewModel> From(IEnumerable<ScpiBalance> items)
        {
            var result = new List<MonthlyScpiViewModel>();
            foreach (var item in items)
            {
                result.Add(MonthlyScpiViewModel.From(item));
            }
            return result;
        }
    }

}
