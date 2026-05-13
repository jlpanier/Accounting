
using Main.ViewModels;

namespace Main.Pages;

[QueryProperty(nameof(AccountId), "accountId")]
public partial class MonthlyBalancesPage : ContentPage
{
    /// <summary>
    /// Numero du compte
    /// </summary>
    public int AccountId { get; set; }

    public MonthlyBalancesPage()
	{
		InitializeComponent();
	}


    /// <summary>
    /// customize behavior immediately prior to the page becoming visible.
    /// </summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is MonthlyBalancesViewModel vm)
        {
            vm.Load(AccountId);
        }
    }
}