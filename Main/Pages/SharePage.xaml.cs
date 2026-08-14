using Main.ViewModels;

namespace Main.Pages;

public partial class SharePage : ContentPage, IQueryAttributable
{
    /// <summary>
    /// Appel avec un compte bancaire en paramètre pour pré-remplir les champs de la page
    /// </summary>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (BindingContext is ShareViewModel vm)
        {
            if (query.TryGetValue("shareId", out var objshare) && objshare is int shareId)
            {
                vm.Init(shareId);
            }
            else
            {
                vm.Init();
            }
        }
    }

    public SharePage()
	{
		InitializeComponent();
        BindingContext = new ShareViewModel()
        {
        };
    }
}