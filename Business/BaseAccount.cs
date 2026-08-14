using Common;
using Repository.Dbo;
using Repository.Entities;

namespace Business
{
    /// <summary>
    /// Gestion des comptes bancaires
    /// </summary>
    public class BaseAccount: IBaseAccounts
    {
        /// <summary>
        /// Operation de compte
        /// </summary>
        public enum AccountType 
        {
            [StringValue("Compte chèque")]
            Cheque,
            [StringValue("Compte épargne")]
            Saving,
            [StringValue("Plan Epargne Action")]
            PEA,
            [StringValue("Plan Epargne Salarial")]
            PEE,
            [StringValue("Assurance Vie")]
            AssuranceVie,
            [StringValue("Bien immobilier (SCPI)")]
            SCPI,
            [StringValue("Overview")]
            Overview
        }

        #region Propriétés

        /// <summary>
        /// Liste de tous les comptes bancaires et ajout d'un bilan
        /// </summary>
        public static List<IBaseAccounts> Accounts
        {
            get
            {
                if (!_accounts.Any())
                {
                    _accounts = new List<IBaseAccounts>();
                    var items = DatabaseAccess.Instance.GetAccounts();

                    foreach (var item in items)
                    {
                        switch ((AccountType)item.Type)
                        {
                            case AccountType.AssuranceVie:
                                _accounts.Add(AssuranceVie.New(item));
                                break;
                            case AccountType.PEA:
                                _accounts.Add(PEA.New(item));
                                break;
                            case AccountType.PEE:
                                _accounts.Add(PEE.New(item));
                                break;
                            case AccountType.Saving:
                                _accounts.Add(SavingAccount.New(item));
                                break;
                            case AccountType.SCPI:
                                _accounts.Add(SCPI.New(item));
                                break;
                            case AccountType.Cheque:
                            default:
                                _accounts.Add(BankAccount.New(item));
                                break;
                        }
                    }
                }
                return _accounts;
            }
        }
        private static List<IBaseAccounts> _accounts = new List<IBaseAccounts>();

        #endregion

        /// <summary>
        /// Création d'un compte bancaire
        /// </summary>
        public static BaseAccount Create(string accountNo, string label, DateTime dtStart, DateTime dtEnd, AccountType accountType)
        {
            var item = new AccountEntity
            {
                AccountNo = accountNo,
                Label = label,
                StartOn = dtStart,
                EndOn = dtEnd,
                DateMaj = DateTime.Now,
                Type = (int)accountType
            };
            DatabaseAccess.Instance.Add(item);
            return new BaseAccount(item);
        }

         /// <summary>
        /// Obtenir un compte bancaire par son numéro de compte
        /// </summary>
        public static IBaseAccounts? GetById(int accountId) => Accounts.FirstOrDefault(a => a.BankAccountId == accountId);

        /// <summary>
        /// Référence vers l'entité de la base de données
        /// </summary>
        public readonly AccountEntity Item;

        /// <summary>
        /// Libellé du compte bancaire
        /// </summary>
        public string Label => Item.Label;

        /// <summary>
        /// Référence du compte bancaire
        /// </summary>
        public int BankAccountId => Item.Id;

        /// <summary>
        /// No du compte bancaire
        /// </summary>
        public string AccountNo => Item.AccountNo;

        /// <summary>
        /// Date d'ouverture du compte bancaire
        /// </summary>
        public DateTime StartOn => Item.StartOn;

        /// <summary>
        /// Date de fermeture du compte bancaire
        /// </summary>
        public DateTime EndOn => Item.EndOn;

        /// <summary>
        /// Operation de compte du compte
        /// </summary>
        public AccountType Type => (AccountType)Item.Type;

        protected BaseAccount(AccountEntity item)
        {
            Item = item;
        }

        protected BaseAccount()
        {
            Item = new AccountEntity()
            {
                AccountNo="",
                DateMaj=DateTime.Now,
                EndOn=DateTime.Now.AddYears(100),
                Label="None",
                StartOn=DateTime.Now,
            };
        }

        /// <summary>
        /// Sauvegarde du compte bancaire
        /// </summary>
        public void Save(string accountNo, string label, DateTime dtStart, DateTime dtEnd, AccountType accountType)
        {
            if (string.IsNullOrEmpty(label))
            {
                throw new ArgumentException("Label is required", nameof(label));
            }
            if (string.IsNullOrWhiteSpace(accountNo))
            {
                throw new ArgumentException("AccountNo is required", nameof(accountNo));
            }
            if (dtStart > dtEnd)
            {
                throw new ArgumentException("Date is required", nameof(dtStart));
            }

            Item.AccountNo = accountNo;
            Item.DateMaj = DateTime.Now;
            Item.Label = label;
            Item.StartOn = dtStart;
            Item.EndOn = dtEnd;
            Item.Type = (int)accountType;
            DatabaseAccess.Instance.Update(Item);
        }

        /// <summary>
        /// Suppression du compte bancaire
        /// </summary>
        public void Delete()
        {
            DatabaseAccess.Instance.Remove(Item);
            _accounts = new List<IBaseAccounts>();
        }
    }
}
