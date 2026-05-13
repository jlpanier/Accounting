using Main.ViewModels;

namespace Main.Pages;

[QueryProperty(nameof(AccountId), "accountId")]
public partial class NewMonthlyBalanceBankAccountPage : ContentPage
{
    /// <summary>
    /// Numero du compte
    /// </summary>
    public int AccountId { get; set; }
    
    public NewMonthlyBalanceBankAccountPage()
	{
		InitializeComponent();
        BindingContext = new NewMonthlyBalanceBankAccountViewModel()
        {
            EffectiveOn = DateTime.Now,
            Balance = 0,
        };
    }

    /// <summary>
    /// customize behavior immediately prior to the page becoming visible.
    /// </summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Animation d’apparition
        Card.Opacity = 0;
        Card.TranslationY = 40;

        await Task.WhenAll(
            Card.FadeToAsync(1, 300, Easing.CubicOut),
            Card.TranslateToAsync(0, 0, 300, Easing.CubicOut)
        );

        if (BindingContext is NewMonthlyBalanceBankAccountViewModel vm)
        {
            vm.Set(AccountId);
        }

    }

    /// <summary>
    /// Sauvegarde des données, avec une animation de clic sur le bouton
    /// </summary>
    private async void OnValidateClicked(object sender, EventArgs e)
    {
        if (BindingContext is NewMonthlyBalanceBankAccountViewModel vm)
        {
            var button = (View)sender;

            await button.ScaleToAsync(0.92, 80, Easing.CubicOut);
            await button.ScaleToAsync(1, 80, Easing.CubicOut);

            // Appel de la commande du ViewModel
            if (vm.SaveCommand?.CanExecute(null) == true)
                vm.SaveCommand.Execute(null);
        }
    }
}