using SQLite;
using System.ComponentModel;

namespace Repository.Entities 
{
    [Table("ORDERS")]
    public partial class OrderEntity : BaseEntity, INotifyPropertyChanged
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
        [Column("ShareId")]
        public int ShareId
        {
            get { return _shareId; }
            set
            {
                if (_shareId != value)
                {
                    _shareId = value;
                    NotifyPropertyChanged(nameof(ShareId));
                }
            }
        }
        private int _shareId;

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

        [Column("Quantity")]
        public double Quantity
        {
            get { return _quantity; }
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    NotifyPropertyChanged(nameof(Quantity));
                }
            }
        }
        private double _quantity;

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

        [Column("Fees")]
        public double Fees
        {
            get { return _fees; }
            set
            {
                if (_fees != value)
                {
                    _fees = value;
                    NotifyPropertyChanged(nameof(Fees));
                }
            }
        }
        private double _fees;

        [Column("Tax")]
        public double Tax
        {
            get { return _tax; }
            set
            {
                if (_tax != value)
                {
                    _tax = value;
                    NotifyPropertyChanged(nameof(Tax));
                }
            }
        }
        private double _tax;

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
