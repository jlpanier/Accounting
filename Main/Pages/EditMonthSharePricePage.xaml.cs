using Main.ViewModels;

namespace Main.Pages;

public partial class EditMonthSharePricePage : ContentPage, IQueryAttributable
{
    /// <summary>
    /// Appel avec un plan epargne entreprise en paramètre pour pré-remplir les champs de la page
    /// </summary>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (BindingContext is EditMonthSharePriceViewModel vm)
        {
            if (query.TryGetValue("BankAccountId", out var objBankAccountId) && objBankAccountId is int bankAccountId
                && query.TryGetValue("EffectiveOn", out var objEffectiveOn) && objEffectiveOn is DateTime effectiveOn
                && query.TryGetValue("ShareId", out var objShareId) && objShareId is int shareId
                )
            {
                vm.Init(bankAccountId, shareId, effectiveOn);
            }
        }
    }

    public EditMonthSharePricePage()
	{
		InitializeComponent();
	}
}