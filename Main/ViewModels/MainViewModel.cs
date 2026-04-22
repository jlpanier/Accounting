using Business;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;


namespace Main.ViewModels
{

    /// <summary>
    /// Gestion de la page principale
    /// </summary>
    public partial class MainViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler? handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        /// <summary>
        /// Evènement de sélection d'une case
        /// </summary>
        public ICommand SquareClickedCommand { get; }

        /// <summary>
        /// Vrai, si affichage du bouton de recherche
        /// </summary>
        public string SolutionText
        {
            get => _solutionText;
            set
            {
                if (value != _solutionText)
                {
                    _solutionText = value;
                    NotifyPropertyChanged(nameof(SolutionText));
                }
            }
        }
        private string _solutionText = "";


        /// <summary>
        /// Vrai, si affichage du bouton de recherche
        /// </summary>
        public bool ShowSearchButton
        {
            get => _showSearchButton;
            set
            {
                if (value != _showSearchButton)
                {
                    _showSearchButton = value;
                    NotifyPropertyChanged(nameof(ShowSearchButton));
                }
            }
        }
        private bool _showSearchButton = true;

        /// <summary>
        /// Vrai, si recherche des solutions en cours
        /// </summary>
        public bool IsSearching
        {
            get => isSearching;
            set
            {
                if (value != isSearching)
                {
                    isSearching = value;
                    NotifyPropertyChanged(nameof(IsSearching));
                }
            }
        }
        private bool isSearching = false;

        /// <summary>
        /// Nombre de colonne/ligne du plateau de jeu
        /// </summary>
        public int Columns
        {
            get => _columns;
            set
            {
                if (value != _columns)
                {
                    _columns = value;
                    NotifyPropertyChanged(nameof(Columns));
                }
            }
        }
        private int _columns = 5;

        /// <summary>
        /// Nombre de cases du plateau de jeu
        /// </summary>
        public int Count => Columns * Columns;

        /// <summary>
        /// Information sur les solutions affichées
        /// </summary>
        public string ButtonText
        {
            get => _buttonText;
            set
            {
                if (value != _buttonText)
                {
                    _buttonText = value;
                    NotifyPropertyChanged(nameof(ButtonText));
                }
            }
        }
        private string _buttonText = "\u2753";

        /// <summary>
        /// Jeu en cours
        /// </summary>
        public Board? Current { get; private set; }

        /// <summary>
        /// Solution possibles
        /// </summary>
        public List<Board> Solutions { get; private set; } = [];

        /// <summary>
        /// ViewModel de chaque case du tableau 
        /// </summary>
        public List<SquareViewModel>? Squares { get; set; }

        public MainViewModel()
        {
            SquareClickedCommand = new Command<SquareViewModel>(OnSquareClicked);
        }

        /// <summary>
        /// Clic sur une case du tableau
        /// </summary>
        private async void OnSquareClicked(SquareViewModel square)
        {
            if(Current!=null && Current.Next(square.Index))
            {
                Debug.Assert(Squares!=null);
                square.Move = Current.Moves.Length-1;
                Refresh();
                if (Current.IsSuccess)
                {
                    if (IsNewSolution(Current.Moves))
                    {
                        Solutions.Add(Current);
                        Current.Save();
                        SolutionText = $"{Solutions.Count} solutions";
                    }
                }
                ShowButtonText();

                await StartAnimation();
            }
        }

        /// <summary>
        /// Changement du nombre de colonnes/lignes du plateau de jeu, avec rechargement des solutions trouvées pour ce nouveau plateau de jeu
        /// </summary>
        /// <param name="columns"></param>
        public void SetColumns(int columns)
        {
            Columns = columns;
            StartNewGane();
            ShowSearchButton = true;
        }

        /// <summary>
        /// Démarre un nouveau jeu
        /// </summary>
        public void StartNewGane()
        {
            Current = Board.Empty(Columns, Columns);
            Refresh();
            LoadSolutions();
        }

        /// <summary>
        /// Chargement des solutions déjà trouvées en base de données 
        /// </summary>
        private void LoadSolutions()
        {
            var worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += (s, e) =>
            {
                Debug.Assert(Current != null);
                Solutions = Current.Load(Columns);
                SolutionText = $"{Solutions.Count} solutions";
                e.Result = Solutions.Count();
            };
            worker.RunWorkerCompleted += (s, e) =>
            {
                ShowButtonText();
            };
            worker.RunWorkerAsync();
        }

        /// <summary>
        /// Prochaine solution à afficher 
        /// </summary>
        public async void Next()
        {
            Debug.Assert(Current != null);
            if (Solutions.Any())
            {
                _indexSolution++;
                if (_indexSolution >= Solutions.Count)
                {
                    _indexSolution = 0;
                }
                Current = Solutions[_indexSolution];
                Refresh();
                ShowSearchButton = false;

                await StartAnimation();
            }
        }

        private async Task StartAnimation()
        {
            if (Current!=null)
            {
                Debug.Assert(Squares != null);
                foreach (var move in Current.Moves)
                {
                    var item = Squares.FirstOrDefault(_ => _.Index == move);
                    if (item != null)
                    {
                        await item.AnimateAsync();
                    }
                }
            }
        }


        /// <summary>
        /// Solution précdante à afficher 
        /// </summary>
        public async void Previous()
        {
            Debug.Assert(Current != null);
            if (Solutions.Any())
            {
                _indexSolution--;
                if (_indexSolution < 0)
                {
                    _indexSolution = Solutions.Count - 1;
                }
                Current = Solutions[_indexSolution];
                Refresh();
                ShowSearchButton = false;

                await StartAnimation();
            }
        }
        private int _indexSolution;

        /// <summary>
        /// Là ou tout commence
        /// </summary>
        public async Task Start()
        {
            if (Current == null)
            {
                StartNewGane();
            }
            else if (Current.IsFailed)
            {
                Next();
            }
            else
            {
                await Compute();
            }
        }

        /// <summary>
        /// Recherche de solutions en tâche de fond, avec mise à jour de l'affichage à chaque nouvelle solution trouvée
        /// </summary>
        private async Task Compute()
        {
            if (IsSearching) return;

            try
            {
                Debug.Assert(Current != null);
                IsSearching = true;


                if (Current.IsSuccess)
                {
                    StartNewGane();
                }
                else if (Current.IsFailed)
                {
                    StartNewGane();
                }
                else
                {
                    if (!subscribed)
                    {
                        Current.NotifyChanged += (s, e) =>
                        {
                            if (IsNewSolution(e))
                            {
                                var newsolution = Board.SaveSolution(e);
                                Solutions.Add(newsolution);
                                SolutionText = $"{Solutions.Count} solutions";
                            }
                        };
                    }
                    await Task.Run(async () =>Current.Compute());
                }
            }
            finally
            {
                if (Solutions.Any())
                {
                    Current = Solutions[0];
                    Refresh();
                }
                IsSearching = false;
                ShowSearchButton = false;
            }
        }
        bool subscribed = false;

        /// <summary>
        /// Affichage du jeu courant
        /// </summary>
        private void Refresh()
        {
            Debug.Assert(Current != null);

            if (Squares==null || !Squares.Any() || Squares.Count != Count)
            {
                int index = 0;
                Squares = new List<SquareViewModel>();
                for (index= 0; index < Count; index++)
                {
                    Squares.Add(new SquareViewModel(index, -99, Current.CanPlay(index), Columns));
                }
                index = 0;
                foreach (var square in Current.Moves)
                {
                    var item = Squares.First(_=>_.Index==square);
                    item.Move = index++;
                }
                NotifyPropertyChanged(nameof(Squares)); // forcer la mise à jour de l'affichage
            }
            else
            {
                var moveitems = Current.Moves.Select(v => (int)v).ToList();
                foreach (var square in Squares)
                {
                    var indextab = moveitems.IndexOf(square.Index);
                    square.Move = indextab >= 0 ? indextab + 1 : -99;
                    square.CanMove = Current.CanPlay(square.Index);
                }
            }
            ShowButtonText();
        }

        /// <summary>
        /// Text du bouton de recherche
        /// </summary>
        private void ShowButtonText()
        {
            Debug.Assert(Current != null);

            if (Current.IsSuccess)
            {
                ButtonText = "Succès";
            }
            else if (Current.IsFailed)
            {
                ButtonText = "Solutions";
            }
            else 
            {
                ButtonText = $"Search";
            }
        }

        /// <summary>
        /// Vraie si la solution proposée est une nouvelle solution
        /// tableau de la solution : l'index du tableau = ordre du mouvement, valeur = index de la case
        /// </summary>
        private bool IsNewSolution(int[] items) => IsNewSolution(items.Select(v => (byte)v).ToArray());

        /// <summary>
        /// Vraie si la solution proposée est une nouvelle solution
        /// tableau de la solution : l'index du tableau = ordre du mouvement, valeur = index de la case
        /// </summary>
        private bool IsNewSolution(byte[] items)
        {
            var movesolution = Encoding.UTF8.GetString(items.ToArray());

            var found = false;
            foreach (var solution in Solutions)
            {
                if (solution.Item.Solution == movesolution)
                {
                    found = true;
                    break;
                }
            }
            return !found;
        }
    }
}
