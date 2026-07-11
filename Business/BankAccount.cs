using Common;
using Repository.Dbo;
using Repository.Entities;

namespace Business
{
    /// <summary>
    /// Gestion des comptes bancaires
    /// </summary>
    public class BankAccount: IBaseAccounts
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
            AssuranceVie
        }

        /// <summary>
        /// Liste de tous les comptes bancaires et ajout d'un bilan
        /// </summary>
        public static List<IBaseAccounts> GetAll(DateTime effectiveOn) 
        {
            double disponible = 0.0;
            double block = 0.0;
            double retirement = 0.0;
            var result = new List<IBaseAccounts>();
            var bankaccounts = DatabaseAccess.Instance.GetBankAccounts().Select(i => new BankAccount(i));

            foreach (var bankaccount in bankaccounts)
            {
                foreach (var balance in bankaccount.Balances.Where(_=>_.EffectiveOn== effectiveOn))
                {
                    switch (bankaccount.Type)
                    {
                        case AccountType.Cheque:
                            disponible += balance.Balance;
                            break;
                        case AccountType.Saving:
                            disponible += balance.Balance;
                            break;
                        case AccountType.PEA:
                        case AccountType.PEE:
                            block += balance.Balance;
                            break;
                        case AccountType.AssuranceVie:
                            retirement += balance.Balance;
                            break;
                    }
                    result.Add(bankaccount);
                }
            }
            result.Add(new OverviewAccounts(disponible, block, retirement));
            return result;        
        } 

        /// <summary>
        /// Création d'un compte bancaire
        /// </summary>
        public static BankAccount Create(string label, string accountNo, DateTime dtStart, DateTime dtEnd, AccountType accountType)
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
            return new BankAccount(item);
        }

        /// <summary>
        /// Création d'un compte bancaire vide
        /// </summary>
        public static BankAccount Empty() => new BankAccount();

        /// <summary>
        /// Obtenir un compte bancaire par son numéro de compte
        /// </summary>
        public static BankAccount? GetByAccountNo(string accountNo)
        {
            var item = DatabaseAccess.Instance.GetBankAccountNo(accountNo);
            return item == null ? null : new BankAccount(item);
        }

        /// <summary>
        /// Obtenir un compte bancaire par son numéro de compte
        /// </summary>
        public static BankAccount? GetByAccountId(int accountId)
        {
            var item = DatabaseAccess.Instance.GetBankAccountId(accountId);
            return item == null ? null : new BankAccount(item);
        }

        /// <summary>
        /// Mise à jour d'un compte bancaire, si le compte n'existe pas, il est créé
        /// </summary>
        public static BankAccount Update(string label, string accountNo, DateTime dtStart, DateTime dtEnd, AccountType accountType)
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
        /// Balance du compte
        /// </summary>
        public double GetBalanceOn(DateTime dt)
        {
            var result = 0.0;
            if (Balances.Any())
            {
                var item = Balances.FirstOrDefault(i=>i.EffectiveOn == dt);
                if (item != null)
                {
                    result = item.Balance;
                }
            }
            return result;
        }

        /// <summary>
        /// Type de compte du compte
        /// </summary>
        public AccountType Type => (AccountType)Item.Type;

        /// <summary>
        /// Chargement des item du compte bancaire
        /// </summary>
        public IEnumerable<BankAccountBalance> Balances
        {
            get 
            {
                if (_balances == null)
                {
                    _balances = DatabaseAccess.Instance.GetMonthlyBalances(BankAccountId).Select(i => new BankAccountBalance(i));
                }
                return _balances;

            }
            private set
            {
                _balances = value;
            }
        }
        private IEnumerable<BankAccountBalance>? _balances;

        private BankAccount(AccountEntity item)
        {
            Item = item;
        }

        private BankAccount()
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
        /// Ajout d'une item du compte bancaire
        /// </summary>
        public BankAccountBalance AddMonthlyBalance(DateTime effectiveOn, double balance)
        {
            var item = Balances.FirstOrDefault(_=>_.EffectiveOn == effectiveOn);
            if (item == null)
            {
                item = BankAccountBalance.Create(BankAccountId, effectiveOn, balance);
            }
            else
            {
                item.Save(effectiveOn,  balance);
            }
            Balances = DatabaseAccess.Instance.GetMonthlyBalances(BankAccountId).Select(i => new BankAccountBalance(i));
            return item;
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
