namespace Main.Pages;

/// <summary>
/// Gestion d'une page de popup simple, avec un message et un bouton de fermeture
/// </summary>
public partial class SimplePopupPage : ContentPage
{
    /// <summary>
    /// Message à afficher dans la popup
    /// </summary>
    private readonly string _message;

    /// <summary>
    /// Gestion d'une page de popup simple, avec un message et un bouton de fermeture
    /// </summary>
    public SimplePopupPage(string message)
	{
		InitializeComponent();
        _message = message;
        MessageLabel.Text = _message;
    }

    /// <summary>
    /// customize behavior immediately prior to the page becoming visible.
    /// </summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Animation fade + scale
        await Task.WhenAll(
            PopupContainer.FadeToAsync(1, 250, Easing.CubicOut),
            PopupContainer.ScaleToAsync(1, 250, Easing.CubicOut)
        );
    }

    /// <summary>
    /// Fermeture de la popup, avec une animation inverse de celle d'ouverture
    /// </summary>
    private async void CloseClicked(object sender, EventArgs e)
    {
        // Animation inverse avant fermeture
        await Task.WhenAll(
            PopupContainer.FadeToAsync(0, 200, Easing.CubicIn),
            PopupContainer.ScaleToAsync(0.8, 200, Easing.CubicIn)
        );

        await Navigation.PopModalAsync();
    }
}