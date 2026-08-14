using Main.ViewModels;

namespace Main.Pages;

public partial class EditDividendePeaPage : ContentPage, IQueryAttributable
{
    /// <summary>
    /// Appel avec un plan epargne entreprise en paramètre pour pré-remplir les champs de la page
    /// </summary>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (BindingContext is EditDividendePeaViewModel vm)
        {
            if (query.TryGetValue("BankAccountId", out var objId) && objId is int bankAccountId)
            {
                if (query.TryGetValue("EffectiveOn", out var objDate) && objDate is DateTime effectiveOn)
                {
                    if (query.TryGetValue("Key", out var objkey) && objkey is int key)
                    {
                        vm.Init(bankAccountId, effectiveOn, key);
                    }
                    else
                    {
                        vm.Init(bankAccountId, effectiveOn);
                    }
                }
            }
        }
    }

    public EditDividendePeaPage()
	{
		InitializeComponent();
	}
}