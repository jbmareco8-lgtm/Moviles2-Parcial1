using System.Collections.ObjectModel;
using Microsoft.Maui.ApplicationModel;
using Parcial1.Core.Models;
using Parcial1.Core.Services;
using Parcial1.Services;

namespace Parcial1.ViewModels;

public class FavoritesViewModel : BaseViewModel
{
    private readonly FavoriteMakeService _favoriteService;
    private readonly DeviceService _deviceService;

    public ObservableCollection<FavoriteMake> Favorites { get; }
        = new();

    private bool _isBusy;

    public bool IsBusy
    {
        get => _isBusy;
        private set
        {
            if (SetProperty(ref _isBusy, value))
            {
                LoadFavoritesCommand.ChangeCanExecute();
                DeleteFavoriteCommand.ChangeCanExecute();
                SelectFavoriteCommand.ChangeCanExecute();
                SaveLocationCommand.ChangeCanExecute();
            }
        }
    }

    private string _statusMessage = string.Empty;

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public Command LoadFavoritesCommand { get; }

    public Command<FavoriteMake> DeleteFavoriteCommand { get; }

    public Command<FavoriteMake> SelectFavoriteCommand { get; }

    public Command<FavoriteMake> SaveLocationCommand { get; }

    public FavoritesViewModel(
        FavoriteMakeService favoriteService,
        DeviceService deviceService)
    {
        _favoriteService = favoriteService;
        _deviceService = deviceService;

        LoadFavoritesCommand = new Command(
            async () => await LoadFavoritesAsync(),
            () => !IsBusy);

        DeleteFavoriteCommand = new Command<FavoriteMake>(
            async favorite => await DeleteFavoriteAsync(favorite),
            favorite => !IsBusy && favorite is not null);

        SelectFavoriteCommand = new Command<FavoriteMake>(
            async favorite => await SelectFavoriteAsync(favorite),
            favorite => !IsBusy && favorite is not null);

        SaveLocationCommand = new Command<FavoriteMake>(
            async favorite => await SaveLocationAsync(favorite),
            favorite => !IsBusy && favorite is not null);
    }

    public async Task LoadFavoritesAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            StatusMessage = "Cargando favoritos...";

            var favorites = await _favoriteService.GetAllAsync();

            Favorites.Clear();

            foreach (var favorite in favorites)
            {
                Favorites.Add(favorite);
            }

            StatusMessage = Favorites.Count > 0
                ? $"Marcas guardadas: {Favorites.Count}"
                : "Todavia no tenes marcas favoritas.";
        }
        catch (Exception)
        {
            StatusMessage = "No se pudieron cargar los favoritos.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task DeleteFavoriteAsync(FavoriteMake? favorite)
    {
        if (favorite is null || IsBusy)
            return;

        try
        {
            IsBusy = true;

            var deleted = await _favoriteService.DeleteAsync(
                favorite.MakeId);

            if (deleted)
            {
                Favorites.Remove(favorite);

                StatusMessage =
                    "Marca eliminada de favoritos.";

                _deviceService.VibrateConfirmation();
            }
            else
            {
                StatusMessage =
                    "No se encontro la marca guardada.";
            }
        }
        catch (Exception)
        {
            StatusMessage = "No se pudo eliminar la marca.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task SelectFavoriteAsync(FavoriteMake? favorite)
    {
        if (favorite is null || IsBusy)
            return;

        try
        {
            IsBusy = true;

            var parameters = new ShellNavigationQueryParameters
            {
                { "makeId", favorite.MakeId },
                { "makeName", favorite.MakeName },
                { "vehicleTypeName", favorite.VehicleTypeName }
            };

            await Shell.Current.GoToAsync(
                nameof(ModelsPage),
                parameters);
        }
        catch (Exception)
        {
            StatusMessage =
                "No se pudo abrir el detalle de la marca.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task SaveLocationAsync(FavoriteMake? favorite)
    {
        if (favorite is null || IsBusy)
            return;

        try
        {
            IsBusy = true;

            StatusMessage = "Obteniendo ubicacion actual...";

            var location = await _deviceService.GetLocationAsync();

            if (location is null)
            {
                StatusMessage =
                    "No se pudo obtener la ubicacion.";

                return;
            }

            var updated = await _favoriteService.UpdateLocationAsync(
                favorite.MakeId,
                location.Latitude,
                location.Longitude);

            if (updated is null)
            {
                StatusMessage =
                    "No se pudo guardar la ubicacion.";

                return;
            }

            var index = Favorites.IndexOf(favorite);

            if (index >= 0)
            {
                Favorites[index] = updated;
            }

            StatusMessage =
                "Ubicacion guardada correctamente.";

            _deviceService.VibrateConfirmation();
        }
        catch (PermissionException)
        {
            StatusMessage =
                "No se puede acceder a la ubicacion sin permiso.";
        }
        catch (UnauthorizedAccessException)
        {
            StatusMessage =
                "No se otorgo el permiso de ubicacion.";
        }
        catch (FeatureNotEnabledException)
        {
            StatusMessage =
                "Activa la ubicacion del dispositivo.";
        }
        catch (FeatureNotSupportedException)
        {
            StatusMessage =
                "Esta funcion requiere un dispositivo movil compatible.";
        }
        catch (Exception)
        {
            StatusMessage =
                "Ocurrio un error al obtener o guardar la ubicacion.";
        }
        finally
        {
            IsBusy = false;
        }
    }
}