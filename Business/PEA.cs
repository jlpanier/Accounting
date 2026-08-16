using Repository.Dbo;
using Repository.Entities;

namespace Business
{
    /// <summary>
    /// Gestion des comptes PEA
    /// </summary>
    public class PEA: BaseAccount
    {
        public static PEA New(AccountEntity item) => new PEA(item);

        public static PEA Empty() => new PEA();

        #region Propriétés

        public List<ITransaction> Transactions 
        { 
            get
            {
                if (_transactions == null)
                {
                    _transactions = new List<ITransaction>();
                    _transactions.AddRange(DatabaseAccess.Instance.GetTransfers(BankAccountId).Select(i => new Transfer(i)));
                    _transactions.AddRange(DatabaseAccess.Instance.GetOrders(BankAccountId).Select(i => new Order(i)));
                    _transactions.AddRange(DatabaseAccess.Instance.GetDividentes(BankAccountId).Select(i => new Dividende(i)));
                }
                return _transactions;
            }
        }
        private List<ITransaction>? _transactions=null;


        #endregion

        public PEA()
        {
        }

        public PEA(AccountEntity item):base(item) 
        {
        }

        /// <summary>
        /// Ajout d'un virement sur le compte
        /// </summary>
        public void AddTransfer(DateTime effectiveOn, double amount)
        {
            var item = Transfer.Create(BankAccountId, effectiveOn, amount);
            Transactions.Add(item);
        }

        /// <summary>
        /// Achat d'une action
        /// </summary>
        public void Purchase(int shareId, DateTime effectiveOn, double quantity, double unitPrice, double fees, double tax)
        {
            var item = Order.Create(shareId, BankAccountId, effectiveOn, quantity, unitPrice, fees, tax);
            Transactions.Add(item);
        }

        /// <summary>
        /// Vente d'action
        /// </summary>
        public void Sell(int shareId, DateTime effectiveOn, double quantity, double unitPrice, double fees, double tax)
        {
            var item = Order.Create(shareId, BankAccountId, effectiveOn, quantity, unitPrice, fees, tax);
            Transactions.Add(item);
        }

        /// <summary>
        /// Dividende d'action
        /// </summary>
        public void Dividende(int shareId, DateTime effectiveOn, double amount)
        {
            var item = Business.Dividende.Create(shareId, BankAccountId, effectiveOn, amount);
            Transactions.Add(item);
        }

        /// <summary>
        /// Comptabiliser le nombre d'actions par actions
        /// </summary>
        public List<MonthlyShare> ShareOn(DateTime effectiveOn)
        {
            var result = new List<MonthlyShare>();
            var orders = Transactions.Where(_=>_.EffectiveOn<=effectiveOn);
            foreach (var itemorder in orders)
            {
                if (itemorder is Order order) 
                {
                    var monthlyshare = result.FirstOrDefault(_ => _.ShareId == order.ShareId);
                    if (monthlyshare == null)
                    {
                        result.Add(new MonthlyShare(order.ShareId, effectiveOn, order));
                    }
                    else 
                    {
                        monthlyshare.Add(order);
                    }
                }
            }
            return result;
        }
    }
}
