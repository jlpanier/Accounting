namespace Business
{
    public class PeaStatut
    {
        public double Cash { get; set; }

        public double Transfer { get; set; }

        public double Dividendes { get; set; }

        public double Valorisation => Groups.Select(_ => _.Valorisation).Sum();

        public double TotalAmount => Valorisation + Cash;

        public List<PeaGroupStatut> Groups { get; set; } = new List<PeaGroupStatut>();
    }
}
