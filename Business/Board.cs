using Repository.Dbo;
using Repository.Entities;
using System.Text;

namespace Business
{
    /// <summary>
    /// Gestion du tableau 
    /// </summary>
    public class Board
    {
        /// <summary>
        /// Evenement de recherche d'une solution
        /// </summary>
        public event EventHandler<int[]>? NotifyChanged;

        /// <summary>
        /// Pour debugger plus facilement, on affiche les valeurs des cases du tableau
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var result = new StringBuilder();
            if (Item != null && !string.IsNullOrWhiteSpace(Item.Solution))
            {
                var bytes = Encoding.UTF8.GetBytes(Item.Solution);
                foreach (var item in bytes)
                {
                    result.Append($"{item} ");
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// Nombre de colonnes maximales
        /// </summary>
        public const int MaxColumns = 10;

        /// <summary>
        /// Nombre de lignes maximales
        /// </summary>
        public const int MaxRows = 10;

        /// <summary>
        /// Nombre de colonnes minimales (pas de solution pour 1..4)
        /// </summary>
        public const int MinColumns = 5;

        /// <summary>
        /// Nombre de lignes minimales (pas de solution pour 1..4)
        /// </summary>
        public const int MinRows = 5;

        /// <summary>
        /// Mouvement possible à partir de la position "index"
        /// </summary>
        public bool PossibleMove { get; private set; }

        /// <summary>
        /// Vrai si toutes les cases du tableau ont été remplies
        /// </summary>
        public bool IsSuccess => Moves.Length == Columns * Columns;

        /// <summary>
        /// Vrai si aucune solution possible 
        /// </summary>
        public bool IsFailed => !IsSuccess && !PossibleMoves.Any();

        /// <summary>
        /// Vrai si aucune solution possible 
        /// </summary>
        public bool CanPlay(int index) => PossibleMoves.Contains(index);

        /// <summary>
        /// Chargement des solutions depuis la base de données
        /// </summary>
        public List<Board> Load(int columns)
        {
            var result = new List<Board>();
            var items = DatabaseAccess.Instance.Get(columns);
            foreach (var item in items)
            {
                result.Add(new Board(item));
            }
            return result;
        }

        /// <summary>
        /// Chargement d'une solution vide
        /// </summary>
        public static Board Empty(int columns, int rows)
        {
            var bytes = new byte[2];
            bytes[0] = 0;
            bytes[1] = (byte)(columns + 2);

            var item = new SolutionEntity()
            {
                Columns = columns,
                Solution = Encoding.UTF8.GetString(bytes),
                DateMaj = DateTime.Now,
            };
            return Create(item);
        }

        /// <summary>
        /// Création d'un bord à partir d'une solution
        /// </summary>
        public static Board Create(SolutionEntity item) => new Board(item);

        /// <summary>
        /// Une solution du tableau
        /// </summary>
        public readonly SolutionEntity Item;

        /// <summary>
        /// Nombre de colonnes du tableau
        /// </summary>
        public int Columns => Item.Columns;

        /// <summary>
        /// Nombre de case total du tableau
        /// </summary>
        public readonly int Total;

        /// <summary>
        /// Solution de ce tableau : la valeur est l'index de la case, et l'indice du tableau correspond à l'ordre de déplacement des cases
        /// </summary>
        public byte[] Moves { get; private set; }

        /// <summary>
        /// Solution de ce tableau : suite de la position "index" des cases
        /// </summary>
        public List<int> PossibleMoves { get; private set; }

        /// <summary>
        /// Création du tableau avec une solution
        /// </summary>
        public Board(SolutionEntity item)
        {
            Item = item;
            Total = Item.Columns * Item.Columns;
            Moves = Encoding.UTF8.GetBytes(item.Solution);
            PossibleMoves = GivePossibleMoves();
        }

        /// <summary>
        /// Donne les mouvements possibles
        /// </summary>
        private List<int> GivePossibleMoves()
        {
            var result = new List<int>();
            var possiblemoves = GivePossibleMoves(Moves[Moves.Length - 1]);
            var moveitems = Moves.Select(v => (int)v).ToList();
            foreach (var possiblemove in possiblemoves)
            {
                if (!moveitems.Contains(possiblemove))
                {
                    result.Add(possiblemove);
                }
            }
            return result;
        }

        /// <summary>
        /// Déplacement possible à partir de la position "index" 
        /// </summary>
        private List<int> GivePossibleMoves(int index)
        {
            var result = new List<int>();

            // x 1 x 2 x
            // 8 x x x 3
            // x x O x x
            // 7 x x x 4
            // x 6 x 5 x
            var col = index % Columns;
            var row = (int)(index / Columns);

            if (row > 1)
            {
                if (col > 0)
                {
                    result.Add(index - 2 * Columns - 1); // ajout de la position 1
                }
                if (col + 1 < Columns)
                {
                    result.Add(index - 2 * Columns + 1); // ajout de la position 2
                }
            }
            if (row > 0)
            {
                if (col > 1)
                {
                    result.Add(index - Columns - 2); // ajout de la position 8
                }
                if (col + 2 < Columns)
                {
                    result.Add(index - Columns + 2); // ajout de la position 3
                }
            }
            if (row + 1 < Columns)
            {
                if (col > 1)
                {
                    result.Add(index + Columns - 2); // ajout de la position 7
                }
                if (col + 2 < Columns)
                {
                    result.Add(index + Columns + 2); // ajout de la position 4
                }
            }
            if (row + 2 < Columns)
            {
                if (col > 0)
                {
                    result.Add(index + Columns + Columns - 1); // ajout de la position 6
                }
                if (col + 1 < Columns)
                {
                    result.Add(index + Columns + Columns + 1); // ajout de la position 5
                }
            }
            return result;
        }

        /// <summary>
        /// Progression du tableau : retourne VRAI si une autre position est possible
        /// </summary>
        public bool Next(int index)
        {
            var result = false;
            if (PossibleMoves.Contains(index))
            {
                Item.Solution += (char)index;
                Moves = Encoding.UTF8.GetBytes(Item.Solution);
                PossibleMoves = GivePossibleMoves();
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Sauvegarde deu tableau
        /// </summary>
        public void Save()
        {
            if (IsSuccess)
            {
                DatabaseAccess.Instance.Insert(Item);
            }
        }

        /// <summary>
        /// Recherche des solutions
        /// </summary>
        public void Compute()
        {
            var moveitems = Moves.Select(v => (int)v).ToArray();
            Next(moveitems);
        }

        /// <summary>
        /// Recherche de la prochaine solution
        /// </summary>
        private void Next(int[] items)
        {
            if (items.Length == Total)
            {
                NotifyChanged?.Invoke(this, items);
            }
            else
            {
                var possiblemoves = GivePossibleMoves(items.Last());
                foreach (var move in possiblemoves)
                {
                    if (move > 0 && move < Total && !items.Contains(move))
                    {
                        Next(items.Append(move).ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// Sauvegarde la solution trouvéeen base de données et notifie les abonnés de l'évènement NotifyChanged 
        /// </summary>
        public static Board SaveSolution(int[] items)
        {
            int columns = (int)(Math.Sqrt(items.Length));
            var item = new SolutionEntity()
            {
                Columns = columns,
                Solution = Encoding.UTF8.GetString(items.Select(v => (byte)v).ToArray()),
                DateMaj = DateTime.Now,
            };
            DatabaseAccess.Instance.Add(item);

            return Board.Create(item);
        }

    }
}
