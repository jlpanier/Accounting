using Repository.Dbo;
using Repository.Entities;

namespace Business
{
    /// <summary>
    /// Gestion des comptes bancaires
    /// </summary>
    public class BankAccount
    {
        /// <summary>
        /// Liste de tous les comptes bancaires
        /// </summary>
        public static List<BankAccount> GetAll()
        {
            var items = DatabaseAccess.Instance.GetBankAccounts();
            return items.Select(i => new BankAccount(i)).ToList();
        }

        /// <summary>
        /// Création d'un compte bancaire
        /// </summary>
        public static BankAccount Create(string label, int accountNo, DateTime dtStart, DateTime dtEnd)
        {
            var item = new AccountEntity
            {
                AccountNo = accountNo,
                Label = label,
                StartOn = dtStart,
                EndOn = dtEnd,
                DateMaj = DateTime.Now
            };
            DatabaseAccess.Instance.Add(item);
            return new BankAccount(item);
        }

        /// <summary>
        /// Obtenir un compte bancaire par son numéro de compte
        /// </summary>
        public static BankAccount? GetByAccountNo(int accountNo)
        {
            var item = DatabaseAccess.Instance.GetBankAccount(accountNo);
            return item == null ? null : new BankAccount(item);
        }

        /// <summary>
        /// Mise à jour d'un compte bancaire, si le compte n'existe pas, il est créé
        /// </summary>
        public static BankAccount Update(string label, int accountNo, DateTime dtStart, DateTime dtEnd)
        {
            if (string.IsNullOrEmpty(label))
            {
                throw new ArgumentException("Label is required", nameof(label));
            }
            if (accountNo < 2)
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
                bankAccount.Save(label, dtStart, dtEnd);
            }
            else
            {
                bankAccount = BankAccount.Create(label, accountNo, dtStart, dtEnd);
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
        /// No du compte bancaire
        /// </summary>
        public int AccountNo => Item.AccountNo;

        /// <summary>
        /// Balance du compte
        /// </summary>
        public double Balance => 999.99;

        /// <summary>
        /// Chargement des balances du compte bancaire
        /// </summary>
        private List<BankAccountBalance> Balances
        {
            get 
            {
                if (_balances == null)
                {
                    _balances = GetBalances().ToList();
                }
                return _balances;

            }
        }
        private List<BankAccountBalance>? _balances;

        public BankAccount(AccountEntity item)
        {
            Item = item;
        }

        /// <summary>
        /// Sauvegarde du compte bancaire
        /// </summary>
        private void Save(string label, DateTime dtStart, DateTime dtEnd)
        {
            Item.DateMaj = DateTime.Now;
            Item.Label = label;
            Item.StartOn = dtStart;
            Item.EndOn = dtEnd;
            DatabaseAccess.Instance.Update(Item);
        }

        /// <summary>
        /// Chargement des balances du compte bancaire
        /// </summary>
        public IEnumerable<BankAccountBalance> GetBalances()
        {
            return DatabaseAccess.Instance.GetMonthlyBalances(AccountNo).Select(i => new BankAccountBalance(i)).ToList();
        }

        /// <summary>
        /// Ajout d'une balance du compte bancaire
        /// </summary>
        public BankAccountBalance AddBalance(DateTime effectiveOn, double balance)
        {
            var item = Balances.FirstOrDefault(_=>_.EffectiveOn == effectiveOn);
            if (item == null)
            {
                item = BankAccountBalance.Create(AccountNo, effectiveOn, balance);
            }
            else
            {
                item.Save(effectiveOn,  balance);
            }
            return item;
        }
    }
}
