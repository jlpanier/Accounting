namespace Business
{
    public class LineTransactions
    {
        public readonly ITransaction Transaction;

        public double Solde { get; private set; }

        public DateTime EffectiveOn => Transaction.EffectiveOn;

        public string Label => Transaction.Label;

        public double Amount => Transaction.Amount;

        public LineTransactions(ITransaction transaction, double solde) 
        {
            Transaction = transaction;
            Solde = solde;
        }
    }
}
