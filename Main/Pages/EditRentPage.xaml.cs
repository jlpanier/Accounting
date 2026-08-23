using Main.ViewModels;

namespace Main.Pages;

public partial class EditRentPage : ContentPage, IQueryAttributable
{
    /// <summary>
    /// Appel pour editer la location d'un apartement
    /// </summary>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (BindingContext is EditRentViewModel vm)
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

    public EditRentPage()
	{
		InitializeComponent();
	}
}