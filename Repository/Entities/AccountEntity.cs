using SQLite;
using System.ComponentModel;

namespace Repository.Entities 
{
    [Table("ACCOUNT")]
    public partial class AccountEntity : BaseEntity, INotifyPropertyChanged
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
        [Unique]
        [Column("Label")]
        public string Label
        {
            get { return _label; }
            set
            {
                if (_label != value)
                {
                    _label = value;
                    NotifyPropertyChanged(nameof(Label));
                }
            }
        }
        private string _label = "";

        [Column("AccountNo")]
        public int AccountNo
        {
            get { return _accountNo; }
            set
            {
                if (_accountNo != value)
                {
                    _accountNo = value;
                    NotifyPropertyChanged(nameof(AccountNo));
                }
            }
        }
        private int _accountNo;

        [Column("StartOn")]
        public DateTime StartOn
        {
            get { return _startOn; }
            set
            {
                if (_startOn != value)
                {
                    _startOn = value;
                    NotifyPropertyChanged(nameof(StartOn));
                }
            }
        }
        private DateTime _startOn;

        [Column("EndOn")]
        public DateTime EndOn
        {
            get { return _endOn; }
            set
            {
                if (_endOn != value)
                {
                    _endOn = value;
                    NotifyPropertyChanged(nameof(EndOn));
                }
            }
        }
        private DateTime _endOn;


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
