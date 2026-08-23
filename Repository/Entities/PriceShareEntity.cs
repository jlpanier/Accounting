using SQLite;
using System.ComponentModel;

namespace Repository.Entities 
{
    [Table("PRICESHARES")]
    public partial class PriceShareEntity : BaseEntity, INotifyPropertyChanged
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

        [Column("UnitPrice")]
        public double UnitPrice
        {
            get { return _amount; }
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    NotifyPropertyChanged(nameof(UnitPrice));
                }
            }
        }
        private double _amount;


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
