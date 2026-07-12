using SQLite;
using System.ComponentModel;

namespace Repository.Entities 
{
    [Table("PEE")]
    public partial class PeeEntity : BaseEntity, INotifyPropertyChanged
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

        [Indexed]
        [Column("BankAccountId")]
        public int BankAccountId
        {
            get { return _bankAccountId; }
            set
            {
                if (_bankAccountId != value)
                {
                    _bankAccountId = value;
                    NotifyPropertyChanged(nameof(BankAccountId));
                }
            }
        }
        private int _bankAccountId;


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

        [Column("Disponible")]
        public double Disponible
        {
            get { return _disponible; }
            set
            {
                if (_disponible != value)
                {
                    _disponible = value;
                    NotifyPropertyChanged(nameof(Disponible));
                }
            }
        }
        private double _disponible;

        [Column("Retirement")]
        public double Retirement
        {
            get { return _retirement; }
            set
            {
                if (_retirement != value)
                {
                    _retirement = value;
                    NotifyPropertyChanged(nameof(Retirement));
                }
            }
        }
        private double _retirement;

        [Column("Blocked")]
        public double Blocked
        {
            get { return _blocked; }
            set
            {
                if (_blocked != value)
                {
                    _blocked = value;
                    NotifyPropertyChanged(nameof(Blocked));
                }
            }
        }
        private double _blocked;


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
