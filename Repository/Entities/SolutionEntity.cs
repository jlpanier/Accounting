using SQLite;
using System.ComponentModel;

namespace Repository.Entities
{
    [Table("SOLUTIONS")]
    public partial class SolutionEntity : BaseEntity, INotifyPropertyChanged
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
        [Column("SolutionId")]
        public int SolutionId
        {
            get { return _solutionId; }
            set
            {
                if (_solutionId != value)
                {
                    _solutionId = value;
                    NotifyPropertyChanged(nameof(SolutionId));
                }
            }
        }
        private int _solutionId;

        [Indexed]
        [Unique]
        [Column("Solution")]
        public string Solution
        {
            get { return _solution; }
            set
            {
                if (_solution != value)
                {
                    _solution = value;
                    NotifyPropertyChanged(nameof(Solution));
                }
            }
        }
        private string _solution = "";

        [Column("Columns")]
        public int Columns
        {
            get { return _columns; }
            set
            {
                if (_columns != value)
                {
                    _columns = value;
                    NotifyPropertyChanged(nameof(Columns));
                }
            }
        }
        private int _columns;

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
