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
            if (query.TryGetValue("item", out var objAccount) && objAccount is Business.SCPI scpi
                && query.TryGetValue("effectiveOn", out var objEffectiveOn) && objEffectiveOn is DateTime effecetiveOn)
            {
                vm.Init(scpi, effecetiveOn);
            }
        }
    }
    public EditScpiPage()
	{
		InitializeComponent();
	}
}