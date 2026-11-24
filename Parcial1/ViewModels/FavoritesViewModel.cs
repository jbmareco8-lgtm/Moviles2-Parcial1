using System.Collections.ObjectModel;
using Parcial1.Core.Models;
using Parcial1.Core.Services;

namespace Parcial1.ViewModels;

public class FavoritesViewModel : BaseViewModel
{
    private readonly FavoriteMakeService _favoriteService;

    public ObservableCollection<FavoriteMake> Favorites { get; } = new();

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

    public FavoritesViewModel(FavoriteMakeService favoriteService)
    {
        _favoriteService = favoriteService;

        LoadFavoritesCommand = new Command(
            async () => await LoadFavoritesAsync(),
            () => !IsBusy);

        DeleteFavoriteCommand = new Command<FavoriteMake>(
            async favorite => await DeleteFavoriteAsync(favorite),
            favorite => !IsBusy && favorite is not null);

        SelectFavoriteCommand = new Command<FavoriteMake>(
            async favorite => await SelectFavoriteAsync(favorite),
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
                StatusMessage = "Marca eliminada de favoritos.";
            }
            else
            {
                StatusMessage = "No se encontro la marca guardada.";
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
            StatusMessage = "No se pudo abrir el detalle de la marca.";
        }
        finally
        {
            IsBusy = false;
        }
    }
}