using Common;
using Repository.Dbo;
using Repository.Entities;

namespace Business
{
    /// <summary>
    /// Gestion des actions
    /// </summary>
    public class Share 
    {
        public enum TypeShare {
            [StringValue("Gestion profilé")]
            Profile,
            [StringValue("Gestion libre")]
            Libre
        }

        /// <summary>
        /// Création d'une action 
        /// </summary>
        public static void Create(string code, string label, TypeShare selectedShareType)
        {
            var newShare = new ShareEntity
            {
                Code = code,
                Label = label,
                Type = (int)selectedShareType,
                DateMaj = DateTime.Now
            };
            DatabaseAccess.Instance.Add(newShare);
            _all.Add(new Share(newShare));
        }

        /// <summary>
        /// Liste de tous les comptes bancaires et ajout d'un bilan
        /// </summary>
        public static List<Share> All
        {
            get
            {
                if (!_all.Any())
                {
                    _all = DatabaseAccess.Instance.GetShares().Select(i => new Share(i)).ToList();
                }
                return _all;
            }
        }
        private static List<Share> _all = new List<Share>();

        /// <summary>
        /// Récupération d'une action par son identifiant
        /// </summary>
        public static Share? GetById(int shareId) => All.FirstOrDefault(s => s.Id == shareId);

        #region Propriétés

        /// <summary>
        /// Entity de l'action
        /// </summary>
        private readonly ShareEntity Item;

        /// <summary>
        /// Clef
        /// </summary>
        public int Id => Item.Id;

        /// <summary>
        /// Code de l'action
        /// </summary>
        public string Code => Item.Code;

        /// <summary>
        /// Libellé de l'action
        /// </summary>
        public string Label => Item.Label;

        /// <summary>
        /// Libellé avec code de l'action
        /// </summary>
        public string Name => $"{Item.Label} ({Item.Code})";

        /// <summary>
        /// Type d'action
        /// </summary>
        public TypeShare Type => (TypeShare)Item.Type;

        #endregion

        public Share(ShareEntity item) 
        {
            Item = item;
        }

        /// <summary>
        /// Sauvegarde
        /// </summary>
        public void Save(string accountno, string label, TypeShare type)
        {
            Item.Code = accountno;
            Item.Label = label;
            Item.Type = (int)type;
            Item.DateMaj = DateTime.Now;
            DatabaseAccess.Instance.Update(Item);
        }

        /// <summary>
        /// Suppression de la balance mensuelle du compte
        /// </summary>
        public void Delete()
        {
            DatabaseAccess.Instance.Remove(Item);
        }
    }
}
