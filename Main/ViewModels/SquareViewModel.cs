using SQLite;
using System.ComponentModel;
using System.Windows.Input;

namespace Main.ViewModels
{
    /// <summary>
    /// Gestion des cases du tableau de jeu
    /// </summary>
    public class SquareViewModel : INotifyPropertyChanged
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
        /// Valeur de la case (mis à jour avec Next)
        /// </summary>
        public string Value
        {
            get => _value;
            set
            {
                if (value != _value)
                {
                    _value = value;
                    NotifyPropertyChanged(nameof(Value));
                }
            }
        }
        private string _value = "";

        /// <summary>
        /// Valeur de la case
        /// </summary>
        public int Move
        {
            get => _move;
            set
            {
                if (value != _move)
                {
                    _move = value;
                    Value = _move < 0 ? " " : _move.ToString();
                    NotifyPropertyChanged(nameof(Move));
                }
            }
        }
        private int _move;

        /// <summary>
        /// Vrai si la case peut être jouée - permet d'entourer la case d'une couleur jaune
        /// </summary>
        public bool CanMove
        {
            get => _canMove;
            set
            {
                if (value != _canMove)
                {
                    _canMove = value;
                    NotifyPropertyChanged(nameof(CanMove));
                }
            }
        }
        private bool _canMove;

        /// <summary>
        /// Index de la case dans le tableau, de 0 à (Columns*Rows-1)
        /// </summary>
        public readonly int Index;

        /// <summary>
        /// Nombre de colonnes du tableau
        /// </summary>
        public readonly int Columns;

        /// <summary>
        /// Ligne de la case dans le tableau
        /// </summary>
        public int Row => (int)(Index / Columns);

        /// <summary>
        /// Colonne de la case dans le tableau
        /// </summary>
        public int Column => Index % Columns;

        public Border? BorderSquare { get; set; }

        public SquareViewModel(int index, int columns)
        {
            Index = index;
            Move = index+1;
            Columns = columns;
            AnimateCommand = new Command(async () => await AnimateAsync());
        }
        public ICommand AnimateCommand { get; }


        public SquareViewModel(int index, int value, bool canMove, int columns)
        {
            Index = index;
            Move = value+1;
            Columns = columns;
            CanMove = canMove;
            AnimateCommand = new Command(async () => await AnimateAsync());
        }

        public async Task AnimateAsync()
        {
            await Task.WhenAll(
                AnimateScaleTo(1.2, 150),
                AnimateRotationTo(150),
                AnimateColorTo(150),
                Task.Delay(150)
            );
            await AnimateScaleTo(1, 150);
        }

        private async Task AnimateScaleTo(double target, uint duration)
        {
            var start = ButtonScale;
            var diff = target - start;
            const int frames = 30;

            for (int i = 0; i < frames; i++)
            {
                ButtonScale = start + diff * (i / (double)frames);
                await Task.Delay((int)(duration / frames));
            }
            ButtonScale = target;
        }

        public double ButtonScale
        {
            get => _buttonScale;
            set
            {
                _buttonScale = value;
                NotifyPropertyChanged(nameof(ButtonScale));
            }
        }
        private double _buttonScale = 1;


        private async Task AnimateRotationTo(uint duration)
        {
           const int frames = 30;

            for (int i = 0; i < frames; i++)
            {
                ButtonRotation = 180 * (i / (double)frames);
                await Task.Delay((int)(duration / frames));
            }
            ButtonRotation = 0;
        }

        public double ButtonRotation
        {
            get => _buttonRotation;
            set
            {
                _buttonRotation = value;
                NotifyPropertyChanged(nameof(ButtonRotation));
            }
        }
        private double _buttonRotation = 1;

        private async Task AnimateColorTo(uint duration)
        {
            ButtonTextColor = Colors.Red;
            await Task.Delay((int)(duration));
            ButtonTextColor = Colors.Blue;
        }

        public Color ButtonTextColor
        {
            get => _buttonTextColor;
            set
            {
                _buttonTextColor = value;
                NotifyPropertyChanged(nameof(ButtonTextColor));
            }
        }
        private Color _buttonTextColor = Colors.Blue;
    }
}
