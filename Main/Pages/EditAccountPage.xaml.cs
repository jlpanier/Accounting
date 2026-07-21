using Main.ViewModels;
using static Business.BaseAccount;

namespace Main.Pages;

/// <summary>
/// Gestion de la page d'ajout d'un compte bancaire
/// </summary>
public partial class EditAccountPage : ContentPage, IQueryAttributable
{
    /// <summary>
    /// Appel avec un compte bancaire en paramètre pour pré-remplir les champs de la page
    /// </summary>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (BindingContext is EditAccountViewModel vm)
        {
            if (query.TryGetValue("item", out var objBankAccount) && objBankAccount is Business.BankAccount bankAccount)
            {
                vm.Init(bankAccount);
            }
            else if (query.TryGetValue("accounttype", out var objAccountType) && objAccountType is AccountType accountType)
            {
                vm.Init(accountType);
            }
        }
    }

    /// <summary>
    /// Gestion de la page d'ajout d'un compte bancaire
    /// </summary>
    public EditAccountPage()
	{
		InitializeComponent();
        BindingContext = new EditAccountViewModel()
        {
             AccountNo="",
             EndDate=new DateTime(2099,12,31),
             Label="",
             StartDate=DateTime.Now.Date,
        };
    }
}