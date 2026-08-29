using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Services;
using FFImageLoading.Maui;
using Main.Converter;
using Main.ViewModels;
using Microsoft.Extensions.Logging;
using Repository.Dbo;
using Syncfusion.Maui.Toolkit.Hosting;

namespace Main
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionToolkit()
                .UseFFImageLoading()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("SegoeUI-Semibold.ttf", "SegoeSemibold");
                    fonts.AddFont("FluentSystemIcons-Regular.ttf", FluentUI.FontFamily);
                    fonts.AddFont("fa-solid-900.ttf", "FontAwesome");
                    fonts.AddFont("MaterialSymbolsRounded.ttf", "MaterialSymbolsRounded");
                });

#if DEBUG
    		builder.Logging.AddDebug();
    		builder.Services.AddLogging(configure => configure.AddDebug());
#endif
            if (Application.Current!=null)
            {
                builder.Services.AddSingleton<IApplication>(Application.Current);
            }
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<IPopupService, PopupService>();
            builder.Services.AddSingleton<IAssetService, AssetService>();
            builder.Services.AddSingleton<IAlertService, AlertService>();
            builder.Services.AddSingleton<DatabaseAccess>();
            builder.Services.AddSingleton<BoolToOpacityConverter>();
            builder.Services.AddSingleton<BoolToFontAttributesConverter>();
            builder.Services.AddSingleton<AccountTypeToTextConverter>();
            builder.Services.AddSingleton<EqualsConverter>();
            builder.Services.AddSingleton<AccountTypeToTextConverter>();

            builder.Services.AddTransient<MainViewModel>();

            return builder.Build();
        }
    }
}
