using Main.ViewModels;
namespace Main.Pages;

public partial class EditPeePage : ContentPage, IQueryAttributable
{
    /// <summary>
    /// Appel avec un plan epargne entreprise en paramètre pour pré-remplir les champs de la page
    /// </summary>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (BindingContext is EditPeeViewModel vm)
        {
            if (query.TryGetValue("item", out var objBankAccount) && objBankAccount is Business.PeeBalance balance)
            {
                vm.Init(balance);
            }
        }
    }

    public EditPeePage()
	{
		InitializeComponent();
        BindingContext = new EditPeeViewModel()
        {
            EffectiveOn = DateTime.Now,
            Disponible = 0,
        };
    }
}