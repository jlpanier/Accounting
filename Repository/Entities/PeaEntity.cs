using SQLite;
using System.ComponentModel;

namespace Repository.Entities 
{
    [Table("PEA")]
    public partial class PeaEntity : BaseEntity, INotifyPropertyChanged
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

        [Column("Cash")]
        public double Cash
        {
            get { return _cash; }
            set
            {
                if (_cash != value)
                {
                    _cash = value;
                    NotifyPropertyChanged(nameof(Cash));
                }
            }
        }
        private double _cash;

        [Column("InvestProfile")]
        public double InvestProfile
        {
            get { return _investProfile; }
            set
            {
                if (_investProfile != value)
                {
                    _investProfile = value;
                    NotifyPropertyChanged(nameof(InvestProfile));
                }
            }
        }
        private double _investProfile;

        [Column("InvestLibre")]
        public double InvestLibre
        {
            get { return _investLibre; }
            set
            {
                if (_investLibre != value)
                {
                    _investLibre = value;
                    NotifyPropertyChanged(nameof(InvestLibre));
                }
            }
        }
        private double _investLibre;

        [Column("AmountProfile")]
        public double AmountProfile
        {
            get { return _amountProfile; }
            set
            {
                if (_amountProfile != value)
                {
                    _amountProfile = value;
                    NotifyPropertyChanged(nameof(AmountProfile));
                }
            }
        }
        private double _amountProfile;

        [Column("AmountLibre")]
        public double AmountLibre
        {
            get { return _amountLibre; }
            set
            {
                if (_amountLibre != value)
                {
                    _amountLibre = value;
                    NotifyPropertyChanged(nameof(AmountLibre));
                }
            }
        }
        private double _amountLibre;

        /// <summary>
        /// Prix unitaire de la part gestion profilé
        /// /// </summary>
        [Column("UnitPrice")]
        public double UnitPrice
        {
            get { return _unitPrice; }
            set
            {
                if (_unitPrice != value)
                {
                    _unitPrice = value;
                    NotifyPropertyChanged(nameof(UnitPrice));
                }
            }
        }
        private double _unitPrice;

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
