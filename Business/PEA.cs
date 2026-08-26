using Common;
using Repository.Dbo;
using Repository.Entities;
using System.Transactions;
using static SQLite.SQLite3;

namespace Business
{
    /// <summary>
    /// Gestion des comptes PEA
    /// </summary>
    public class PEA: BaseAccount
    {
        /// <summary>
        /// Création d'un compte bancaire
        /// </summary>
        public static PEA Create(string label, string accountNo, DateTime dtStart, DateTime dtEnd)
        {
            var baseaccount = Create(label, accountNo, dtStart, dtEnd, AccountType.PEA);
            return new PEA(baseaccount.Item);
        }

        /// <summary>
        /// Utilsé lors du chargement des comptes
        /// </summary>
        public static PEA New(AccountEntity item) => new PEA(item);

        /// <summary>
        /// pour l'initialisation
        /// </summary>
        public static PEA Empty() => new PEA();

        #region Propriétés

        /// <summary>
        /// Transactions du compte PEA
        /// </summary>
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

        private PEA(AccountEntity item):base(item) 
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

        /// <summary>
        /// Valeur titre
        /// </summary>
        public PeaStatut StatutOn(DateTime effectiveOn)
        {
            List<MonthlyShare> shares = ShareOn(effectiveOn);
            List<PeaGroupStatut> groups = new List<PeaGroupStatut>();
            foreach (Share.TypeShare accountType in Enum.GetValues(typeof(Share.TypeShare)))
            {
                groups.Add(new PeaGroupStatut()
                {
                    Transfer=0,
                    Valorisation=0,
                    Label= accountType.GetStringValue(),
                    Type = accountType
                });
            }
            double cash = 0;
            double transfer = 0;
            double dividendes = 0;
            foreach (var transaction in Transactions.Where(_ => _.EffectiveOn <= effectiveOn))
            {
                cash += transaction.Amount;
                if (transaction is Transfer itemtransfer) transfer += itemtransfer.Amount;
                else if (transaction is Dividende dividendeitem) dividendes += dividendeitem.Amount;
                else if (transaction is Order itemorder)
                {
                    var share = itemorder.Share;
                    if (share != null)
                    {
                        var group = groups.First(_ => _.Type == share.Type);
                        switch (itemorder.Type)
                        {
                            case Order.OrderType.Buy:
                                group.Transfer -= itemorder.Amount;
                                break;
                            case Order.OrderType.Sell:
                                group.Transfer += itemorder.Amount;
                                break;
                        }
                    }
                }
            }

            foreach (var group in groups) 
            {
                var monthlyshare = shares.Where(_ => _.Item.Type == group.Type);
                group.Valorisation = monthlyshare.Select(_=>_.Amount).Sum();
            }

            return new PeaStatut()
            {
                Cash=cash,
                Transfer= transfer,
                Groups=groups,
                Dividendes = dividendes
            };
        }


        /// <summary>
        /// Somme des dividendes entre ces deux dates
        /// </summary>
        /// <param name="starton"></param>
        /// <param name="endon"></param>
        /// <returns></returns>
        public double GetDividendes(DateTime starton, DateTime endon) => Transactions.Where(_ => _.GetType() == typeof(Dividende) && _.EffectiveOn > starton && _.EffectiveOn < endon).Select(_ => _.Amount).Sum();
    }
}
