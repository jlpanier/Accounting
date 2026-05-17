using Main.ViewModels;
using static Business.BankAccount;

namespace Main.Pages;

public partial class SelectTypeAccountPage : ContentPage, IQueryAttributable
{
    /// <summary>
    /// Appel avec un type de compte bancaire en paramètre pour pré-remplir les champs de la page
    /// </summary>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (BindingContext is SelectTypeAccountViewModel vm && query.TryGetValue("accounttype", out var obj) && obj is AccountType item)
        {
            vm.Init(item);
        }
    }

    /// <summary>
    /// customize behavior immediately prior to the page becoming visible.
    /// </summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();
    }

    public SelectTypeAccountPage()
	{
		InitializeComponent();
        BindingContext = new SelectTypeAccountViewModel();
    }

}