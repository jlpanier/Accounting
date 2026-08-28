using Main.ViewModels;

namespace Main.Pages;

public partial class EditSettingPage : ContentPage, IQueryAttributable
{
    /// <summary>
    /// Appel avec un compte bancaire en paramètre pour pré-remplir les champs de la page
    /// </summary>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (BindingContext is EditSettingViewModel vm)
        {
            if (query.TryGetValue("Key", out var objId) && objId is string key)
            {
                vm.Init(key);
            }
        }
    }

    public EditSettingPage()
	{
		InitializeComponent();
	}
}