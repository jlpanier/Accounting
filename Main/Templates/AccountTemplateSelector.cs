using Main.ViewModels;

namespace Main.Templates
{
    /// <summary>
    /// Selection du template en fonction du viewmodel (type de compte)
    /// </summary>
    public class AccountTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Template compte courant
        /// </summary>
        public required DataTemplate BankTemplate { get; set; }

        /// <summary>
        /// Template compte d'épargne
        /// </summary>
        public required DataTemplate SavingTemplate { get; set; }

        /// <summary>
        /// Template pour PEE
        /// </summary>
        public required DataTemplate PeeTemplate { get; set; }

        /// <summary>
        /// Template pour PEA
        /// </summary>
        public required DataTemplate PeaTemplate { get; set; }

        /// <summary>
        /// Template pour SCPI
        /// </summary>
        public required DataTemplate ScpiTemplate { get; set; }

        /// <summary>
        /// Template pour apartement
        /// </summary>
        public required DataTemplate AppartmentTemplate { get; set; }


        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return item switch
            {
                SummaryBankAccountViewModel => BankTemplate,
//                MonthlyBalancesViewModel => SavingTemplate,
                SummaryPeeViewModel => PeeTemplate,
                SummaryPeaViewModel => PeaTemplate,
                SummaryScpiViewModel => ScpiTemplate,
                SummaryRentViewModel => AppartmentTemplate,
                _ => BankTemplate
            };
        }
    }

}
