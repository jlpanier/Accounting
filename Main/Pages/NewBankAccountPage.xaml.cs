using ExCSS;
using Main.ViewModels;

namespace Main.Pages;

/// <summary>
/// Gestion de la page d'ajout d'un compte bancaire
/// </summary>
public partial class NewBankAccountPage : ContentPage, IQueryAttributable
{
    /// <summary>
    /// Appel avec un compte bancaire en paramètre pour pré-remplir les champs de la page
    /// </summary>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("item", out var obj) && obj is Business.BankAccount item && BindingContext is NewBankAcccountViewModel vm)
        {
            vm.Init(item);
        }
    }

    /// <summary>
    /// Gestion de la page d'ajout d'un compte bancaire
    /// </summary>
    public NewBankAccountPage()
	{
		InitializeComponent();
        BindingContext = new NewBankAcccountViewModel()
        {
             AccountNo=0,
             EndDate=new DateTime(2099,12,31),
             Label="",
             StartDate=DateTime.Now.Date,
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
    }

    /// <summary>
    /// Sauvegarde des données, avec une animation de clic sur le bouton
    /// </summary>
    private async void OnValidateClicked(object sender, EventArgs e)
    {
        if (BindingContext is NewBankAcccountViewModel vm)
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