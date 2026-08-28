using SQLite;
using System.ComponentModel;

namespace Repository.Entities 
{
    [Table("SETTINGS")]
    public partial class SettingsEntity : BaseEntity, INotifyPropertyChanged
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
        [Column("Key")]
        public string Key
        {
            get { return _key; }
            set
            {
                if (_key != value)
                {
                    _key = value;
                    NotifyPropertyChanged(nameof(Key));
                }
            }
        }
        private string _key = "";

        [Column("Val")]
        public string Val
        {
            get { return _val; }
            set
            {
                if (_val != value)
                {
                    _val = value;
                    NotifyPropertyChanged(nameof(Val));
                }
            }
        }
        private string _val="";

        [Column("Desc")]
        public string Desc
        {
            get { return _desc; }
            set
            {
                if (_desc != value)
                {
                    _desc = value;
                    NotifyPropertyChanged(nameof(Desc));
                }
            }
        }
        private string _desc="";

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
