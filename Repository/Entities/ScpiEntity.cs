using SQLite;
using System.ComponentModel;

namespace Repository.Entities 
{
    [Table("SCPI")]
    public partial class ScpiEntity : BaseEntity, INotifyPropertyChanged
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

        /// <summary>
        /// Identifiant unique de l'entité
        /// </summary>
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

        /// <summary>
        /// Identifiant du compte associé à l'entité
        /// </summary>
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

        /// <summary>
        /// Date de valeur
        /// </summary>
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

        /// <summary>
        /// Nombre de parts détenues
        /// </summary>
        [Column("NumberOfShares")]
        public int NumberOfShares
        {
            get { return _numberOfShares; }
            set
            {
                if (_numberOfShares != value)
                {
                    _numberOfShares = value;
                    NotifyPropertyChanged(nameof(NumberOfShares));
                }
            }
        }
        private int _numberOfShares;

        /// <summary>
        /// Prix unitaire de la part
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

        /// <summary>
        /// Loyer mensuel perçu
        /// </summary>
        [Column("Rent")]
        public double Rent
        {
            get { return _monthlyRent; }
            set
            {
                if (_monthlyRent != value)
                {
                    _monthlyRent = value;
                    NotifyPropertyChanged(nameof(Rent));
                }
            }
        }
        private double _monthlyRent;

        /// <summary>
        /// Date delamise à jour
        /// </summary>
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
