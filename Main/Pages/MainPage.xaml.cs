using Main.ViewModels;

namespace Main.Pages
{
    /// <summary>
    /// Gestion de la page principale, avec le plateau de jeu et les boutons de contrôle
    /// </summary>
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        /// <summary>
        /// customize behavior immediately prior to the page becoming visible.
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();

        }


    }
}