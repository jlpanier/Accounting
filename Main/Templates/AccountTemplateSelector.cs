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
        /// Template bilan global
        /// </summary>
        public required DataTemplate OverviewTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return item switch
            {
                BankAccountViewModel => BankTemplate,
                MonthlyBalancesViewModel => SavingTemplate,
                OverviewViewModel => OverviewTemplate,
                _ => BankTemplate
            };
        }
    }

}
