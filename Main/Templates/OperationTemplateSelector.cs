using Main.ViewModels;
using static Main.ViewModels.EditPeaViewModel;

namespace Main.Templates
{
    public class OperationTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? TransferTemplate { get; set; }
        public DataTemplate? StockTradeTemplate { get; set; }
        public DataTemplate? DividendTemplate { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            if (item is EditPeaViewModel vm)
            {
                return vm.SelectedOperationType switch
                {
                    OperationType.Virement => TransferTemplate,
                    OperationType.Achat => StockTradeTemplate,
                    OperationType.Vente => StockTradeTemplate,
                    OperationType.Dividende => DividendTemplate,
                    _ => TransferTemplate
                };
            }
            else if (item is Business.Transfer)
            {
                return TransferTemplate;
            }
            else if (item is Business.Order)
            {
                return StockTradeTemplate;
            }
            else if (item is Business.Dividende)
            {
                return DividendTemplate;
            }

            return TransferTemplate;
        }
    }

}
