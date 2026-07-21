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
        /// Type de compte
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

        /// <summary>
        /// Liste de tous les comptes bancaires et ajout d'un bilan
        /// </summary>
        public static List<IBaseAccounts> GetAll(DateTime effectiveOn) 
        {
            double disponible = 0.0;
            double blocked = 0.0;
            double retirement = 0.0;
            var result = new List<IBaseAccounts>();
            var items = DatabaseAccess.Instance.GetAccounts();

            foreach (var item in items)
            {
                IBaseAccounts bankaccount;
                switch ((AccountType)item.Type)
                {
                    case AccountType.AssuranceVie:
                        var assurancevie = AssuranceVie.New(item);
                        retirement += assurancevie.GetBalanceOn(effectiveOn);
                        bankaccount = assurancevie;
                        break;
                    case AccountType.PEA:
                    case AccountType.PEE:
                        var pee = new PEE(item);
                        var balance= pee.GetBalance(effectiveOn);
                        disponible += balance.Disponible;
                        retirement += balance.Retirement;
                        blocked += balance.Blocked;
                        bankaccount = pee;
                        break;
                    case AccountType.Saving:
                        var savingaccount = SavingAccount.New(item);
                        disponible += savingaccount.GetBalanceOn(effectiveOn);
                        bankaccount = savingaccount;
                        break;
                    case AccountType.SCPI:
                        bankaccount = SCPI.New(item);
                        break;
                    case AccountType.Cheque:
                    default:
                        var account = BankAccount.New(item);
                        disponible += account.GetBalanceOn(effectiveOn);
                        bankaccount = account;
                        break;
                }
                result.Add(bankaccount);
            }
            result.Add(new OverviewAccounts(disponible, blocked, retirement));
            return result;
        }

        /// <summary>
        /// Création d'un compte bancaire
        /// </summary>
        public static BaseAccount Create(string label, string accountNo, DateTime dtStart, DateTime dtEnd, AccountType accountType)
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
        /// Création d'un compte bancaire vide
        /// </summary>
        public static BaseAccount Empty() => new BaseAccount();

        /// <summary>
        /// Obtenir un compte bancaire par son numéro de compte
        /// </summary>
        public static BaseAccount? GetByAccountNo(string accountNo)
        {
            var item = DatabaseAccess.Instance.GetBankAccountNo(accountNo);
            if (item == null)
            {
                return null;
            }
            else
            {
                switch ((AccountType)item.Type)
                {
                    case AccountType.Cheque:
                        return BankAccount.New(item);
                    case AccountType.Saving:
                        break;
                    default:
                        break;
                }
                return item == null ? null : new BaseAccount(item);
            }
        }

        /// <summary>
        /// Obtenir un compte bancaire par son numéro de compte
        /// </summary>
        public static IBaseAccounts? GetByAccountId(int accountId)
        {
            var item = DatabaseAccess.Instance.GetBankAccountId(accountId);
            if (item == null)
            {
                return null;
            }
            else
            {
                switch ((AccountType)item.Type)
                {
                    case AccountType.Cheque:
                        return BankAccount.New(item);
                    case AccountType.Saving:
                        return SavingAccount.New(item); 
                    case AccountType.PEA:
                        break;
                    case AccountType.PEE:
                        return PEE.New(item);
                    case AccountType.SCPI:
                        return SCPI.New(item);
                    case AccountType.AssuranceVie:
                        return AssuranceVie.New(item);
                }
            }
            return item == null ? null : new BaseAccount(item);
        }

        /// <summary>
        /// Mise à jour d'un compte bancaire, si le compte n'existe pas, il est créé
        /// </summary>
        public static BaseAccount Update(string label, string accountNo, DateTime dtStart, DateTime dtEnd, AccountType accountType)
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
            var bankAccount = GetByAccountNo(accountNo);
            if (bankAccount != null)
            {
                bankAccount.Save(label, dtStart, dtEnd, accountType);
            }
            else
            {
                bankAccount = BankAccount.Create(label, accountNo, dtStart, dtEnd, accountType);
            }
            return bankAccount;
        }

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
        /// Type de compte du compte
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
        private void Save(string label, DateTime dtStart, DateTime dtEnd, AccountType accountType)
        {
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
        }
    }
}
