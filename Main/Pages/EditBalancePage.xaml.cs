using Main.ViewModels;

namespace Main.Pages;

/// <summary>
/// Gestion des balances mensuelles d'un compte bancaire en paramètre pour pré-remplir les champs de la page
/// </summary>
public partial class EditBalancePage : ContentPage, IQueryAttributable
{
    /// <summary>
    /// Appel avec un compte bancaire en paramètre pour pré-remplir les champs de la page
    /// </summary>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (BindingContext is EditBalanceViewModel vm)
        {
            if (query.TryGetValue("item", out var obj) && obj is Business.BankAccountBalance item)
            {
                vm.Init(item);
            }
        }
    }
    
    public EditBalancePage()
	{
		InitializeComponent();
        BindingContext = new EditBalanceViewModel()
        {
            EffectiveOn = DateTime.Now,
            Balance = 0,
        };
    }
}