using SQLite;
using System.ComponentModel;

namespace Repository.Entities 
{
    [Table("RENT")]
    public partial class MonthlyRentEntity : BaseEntity, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            IsDirty = true;
            PropertyChangedEventHandler? handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        [Ignore]
        public bool IsDirty { get; set; }

        #endregion

        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id
        {
            get { return _Id; }
            set
            {
                if (_Id != value)
                {
                    _Id = value;
                    NotifyPropertyChanged(nameof(Id));
                }
            }
        }
        private int _Id;

        [Column("AppartmentId")]
        public int AppartmentId
        {
            get { return _appartmentId; }
            set
            {
                if (_appartmentId != value)
                {
                    _appartmentId = value;
                    NotifyPropertyChanged(nameof(AppartmentId));
                }
            }
        }
        private int _appartmentId;

        [Column("EffectiveOn")]
        public DateTime EffectiveOn
        {
            get { return _effectiveOn; }
            set
            {
                if (_effectiveOn != value)
                {
                    _effectiveOn = value;
                    NotifyPropertyChanged(nameof(EffectiveOn));
                }
            }
        }
        private DateTime _effectiveOn;

        [Column("Renter")]
        public string Renter
        {
            get { return _renter; }
            set
            {
                if (_renter != value)
                {
                    _renter = value;
                    NotifyPropertyChanged(nameof(Renter));
                }
            }
        }
        private string _renter=string.Empty;

        [Column("Rent")]
        public double Rent
        {
            get { return _rent; }
            set
            {
                if (_rent != value)
                {
                    _rent = value;
                    NotifyPropertyChanged(nameof(Rent));
                }
            }
        }
        private double _rent;

        [Column("Provision")]
        public double Provision
        {
            get { return _provision; }
            set
            {
                if (_provision != value)
                {
                    _provision = value;
                    NotifyPropertyChanged(nameof(Provision));
                }
            }
        }
        private double _provision;

        [Column("InOut")]
        public double InOut
        {
            get { return _inout; }
            set
            {
                if (_inout != value)
                {
                    _inout = value;
                    NotifyPropertyChanged(nameof(InOut));
                }
            }
        }
        private double _inout;

        [Column("Work")]
        public double Work
        {
            get { return _work; }
            set
            {
                if (_work != value)
                {
                    _work = value;
                    NotifyPropertyChanged(nameof(Work));
                }
            }
        }
        private double _work;

        [Column("Exceptionel")]
        public double Exceptionel
        {
            get { return _exceptionnel; }
            set
            {
                if (_exceptionnel != value)
                {
                    _exceptionnel = value;
                    NotifyPropertyChanged(nameof(Exceptionel));
                }
            }
        }
        private double _exceptionnel;

        [Column("Garantee")]
        public double Garantee
        {
            get { return _garantee; }
            set
            {
                if (_garantee != value)
                {
                    _garantee = value;
                    NotifyPropertyChanged(nameof(Garantee));
                }
            }
        }
        private double _garantee;

        [Column("Gestion")]
        public double Gestion
        {
            get { return _gestion; }
            set
            {
                if (_gestion != value)
                {
                    _gestion = value;
                    NotifyPropertyChanged(nameof(Gestion));
                }
            }
        }
        private double _gestion;

        [Column("Syndic")]
        public double Syndic
        {
            get { return _syndic; }
            set
            {
                if (_syndic != value)
                {
                    _syndic = value;
                    NotifyPropertyChanged(nameof(Syndic));
                }
            }
        }
        private double _syndic;

        [Column("Transfer")]
        public double Transfer
        {
            get { return _transfer; }
            set
            {
                if (_transfer != value)
                {
                    _transfer = value;
                    NotifyPropertyChanged(nameof(Transfer));
                }
            }
        }
        private double _transfer;

        [Column("DateMaj")]
        public DateTime DateMaj
        {
            get { return _datemaj; }
            set
            {
                if (_datemaj != value)
                {
                    _datemaj = value;
                    NotifyPropertyChanged(nameof(DateMaj));
                }
            }
        }
        private DateTime _datemaj;
    }
}
