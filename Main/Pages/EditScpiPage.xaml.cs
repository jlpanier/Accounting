using Main.ViewModels;

namespace Main.Pages;

public partial class EditScpiPage : ContentPage, IQueryAttributable
{
    /// <summary>
    /// Appel avec un plan epargne entreprise en paramètre pour pré-remplir les champs de la page
    /// </summary>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (BindingContext is EditScpiViewModel vm)
        {
            if (query.TryGetValue("BankAccountId", out var objId) && objId is int bankAccountId)
            {
                if (query.TryGetValue("EffectiveOn", out var objDate) && objDate is DateTime dt)
                {
                    vm.Init(bankAccountId, dt);
                }
            }
        }
    }
    public EditScpiPage()
	{
		InitializeComponent();
	}
}