using Microsoft.Extensions.Logging;
using Parcial1.Services;
using Parcial1.ViewModels;

namespace Parcial1;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddHttpClient<IApiService, ApiService>(client =>
        {
            client.BaseAddress =
                new Uri("https://vpic.nhtsa.dot.gov/api/vehicles/");
        });

        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<ModelsViewModel>();
        builder.Services.AddTransient<ModelsPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}