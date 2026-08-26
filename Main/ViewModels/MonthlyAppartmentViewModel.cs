using Business;
using System.Collections.ObjectModel;

namespace Main.ViewModels
{
    public class MonthlyAppartmentViewModel : BaseViewModel
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
        /// Occupant de l'appartement
        /// </summary>
        public string Renter
        {
            get => _renter;
            set
            {
                if (_renter != value)
                {
                    _renter = value;
                    NotifyPropertyChanged(nameof(Renter));
                }
            }
        }
        public string _renter = "";

        /// <summary>
        /// Loyer perçu sur la période
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
        /// Charge de l'appartement sur la période
        /// </summary>
        public double Provision
        {
            get => _provision;
            set
            {
                if (_provision != value)
                {
                    _provision = value;
                    NotifyPropertyChanged(nameof(Provision));
                }
            }
        }
        public double _provision = 0;

        /// <summary>
        /// Frais entrée/sortie sur la période (loyer - charges)
        /// </summary>
        public double InOut
        {
            get => _inout;
            set
            {
                if (_inout != value)
                {
                    _inout = value;
                    NotifyPropertyChanged(nameof(InOut));
                }
            }
        }
        public double _inout = 0.0;

        /// <summary>
        /// Travaux réalisés sur la période
        /// </summary>
        public double Work
        {
            get => _work;
            set
            {
                if (_work != value)
                {
                    _work = value;
                    NotifyPropertyChanged(nameof(Work));
                }
            }
        }
        public double _work = 0.0;

        /// <summary>
        /// Frais de garantee sur la période
        /// </summary>
        public double Garantee
        {
            get => _garantee;
            set
            {
                if (_garantee != value)
                {
                    _garantee = value;
                    NotifyPropertyChanged(nameof(Garantee));
                }
            }
        }
        public double _garantee = 0.0;

        /// <summary>
        /// Frais de gestion
        /// </summary>
        public double Gestion
        {
            get => _gestion;
            set
            {
                if (_gestion != value)
                {
                    _gestion = value;
                    NotifyPropertyChanged(nameof(Gestion));
                }
            }
        }
        public double _gestion = 0.0;

        /// <summary>
        /// Fraisdu syndic sur la période
        /// </summary>
        public double Syndic
        {
            get => _syndic;
            set
            {
                if (_syndic != value)
                {
                    _syndic = value;
                    NotifyPropertyChanged(nameof(Syndic));
                }
            }
        }
        public double _syndic = 0.0;

        /// <summary>
        /// Frais exceptionnels sur la période (travaux + charges + frais de gestion)
        /// </summary>
        public double Exceptionel
        {
            get => _exceptionel;
            set
            {
                if (_exceptionel != value)
                {
                    _exceptionel = value;
                    NotifyPropertyChanged(nameof(Exceptionel));
                }
            }
        }
        public double _exceptionel = 0.0;

        /// <summary>
        /// Virement
        /// </summary>
        public double Transfer
        {
            get => _transfer;
            set
            {
                if (_transfer != value)
                {
                    _transfer = value;
                    NotifyPropertyChanged(nameof(Transfer));
                }
            }
        }
        public double _transfer;

        /// <summary>
        /// Virement
        /// </summary>
        public double Credit => Rent + Provision;

        /// <summary>
        /// Virement
        /// </summary>
        public double Debit => InOut+ Work+ Exceptionel + Garantee + Gestion + Syndic;

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
        public static MonthlyAppartmentViewModel From(MonthlyRent item) => new MonthlyAppartmentViewModel()
        {
            BankAccountId = item.BankAccountId,
            Exceptionel = item.Exceptionel,
            Garantee = item.Garantee,
            InOut = item.InOut,
            Provision = item.Provision,
            Rent = item.Rent,
            Renter = item.Renter,
            Syndic = item.Syndic,
            Transfer = item.Transfer,
            Work = item.Work,
            EffectiveOn=item.EffectiveOn,
            Gestion=item.Gestion
        };

        /// <summary>
        /// Conversion en MonthlyRent -> MonthlyAppartmentViewModel
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static List<MonthlyAppartmentViewModel> From(IEnumerable<MonthlyRent> items)
        {
            var result = new List<MonthlyAppartmentViewModel>();
            foreach(var item in items)
            {
                result.Add(MonthlyAppartmentViewModel.From(item));
            }
            return result;
        }
    }
}
