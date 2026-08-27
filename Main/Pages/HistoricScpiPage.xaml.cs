using Main.ViewModels;

namespace Main.Pages;

public partial class HistoricScpiPage : ContentPage, IQueryAttributable
{
    /// <summary>
    /// Appel avec un compte bancaire en paramètre pour pré-remplir les champs de la page
    /// </summary>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (BindingContext is HistoricScpiViewModel vm)
        {
            if (query.TryGetValue("BankAccountId", out var objBankAccount) && objBankAccount is int bankAccountId)
            {
                vm.Init(bankAccountId);
            }
        }
    }

    public HistoricScpiPage()
	{
		InitializeComponent();
	}
}