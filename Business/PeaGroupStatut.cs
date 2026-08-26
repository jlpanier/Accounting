
namespace Business
{
    public class PeaGroupStatut
    {
        public Share.TypeShare Type { get; set; }

        public string Label { get; set; } = "";

        public double Transfer { get; set; }

        public double Valorisation { get; set; }

        public double Gain => Transfer > 0 ? 100 * (Valorisation-Transfer) / Transfer : 0.0;
    }
}
