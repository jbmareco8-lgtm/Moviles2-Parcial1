using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;
using Parcial1.Services;
using Parcial1.ViewModels;
using Parcial1.Core.Repositories;
using Parcial1.Core.Services;

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

        // Base de datos local
        builder.Services.AddSingleton<DatabaseService>(services =>
        {
            var dbPath = Path.Combine(
                FileSystem.AppDataDirectory,
                "favoritos.db3");

            return new DatabaseService(dbPath);
        });

        // Repositorio y servicio de favoritos
        builder.Services.AddSingleton<IFavoriteMakeRepository,
            FavoriteMakeRepository>();

        builder.Services.AddTransient<FavoriteMakeService>();

        // ViewModels y paginas
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddSingleton<MainPage>();

        builder.Services.AddTransient<ModelsViewModel>();
        builder.Services.AddTransient<ModelsPage>();

        builder.Services.AddTransient<FavoritesViewModel>();
        builder.Services.AddTransient<FavoritesPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}