using Main.ViewModels;

namespace Main.Pages;

public partial class EditPeaPage : ContentPage, IQueryAttributable
{
    /// <summary>
    /// Appel avec un plan epargne entreprise en paramètre pour pré-remplir les champs de la page
    /// </summary>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (BindingContext is EditPeaViewModel vm)
        {
            if (query.TryGetValue("BankAccountId", out var objId) && objId is int bankAccountId)
            {
                if (query.TryGetValue("EffectiveOn", out var objDate) && objDate is DateTime dt)
                {
                    if (query.TryGetValue("Key", out var objkey) && objkey is int key)
                    {
                        //vm.Init(key, bankAccountId, dt);
                    }
                    else
                    {
                        //vm.Init(bankAccountId, dt);
                    }
                }
            }
        }
    }

    public EditPeaPage()
	{
		InitializeComponent();
        BindingContext = new EditPeaViewModel()
        {
            EffectiveOn = DateTime.Now,
        };
    }
}