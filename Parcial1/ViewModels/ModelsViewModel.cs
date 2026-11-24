using System.Collections.ObjectModel;
using Parcial1.Models;
using Parcial1.Services;
using Parcial1.Core.Models;
using Parcial1.Core.Services;

namespace Parcial1.ViewModels;

public class ModelsViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IApiService _api;
    private readonly FavoriteMakeService _favoriteService;

    public ObservableCollection<CarModel> Models { get; } = new();

    private int _makeId;

    public int MakeId
    {
        get => _makeId;
        set
        {
            if (SetProperty(ref _makeId, value))
            {
                SaveFavoriteCommand?.ChangeCanExecute();
            }
        }
    }

    private string _makeName = string.Empty;

    public string MakeName
    {
        get => _makeName;
        set => SetProperty(ref _makeName, value);
    }

    private string _vehicleTypeName = string.Empty;

    public string VehicleTypeName
    {
        get => _vehicleTypeName;
        set => SetProperty(ref _vehicleTypeName, value);
    }

    private bool _isBusy;

    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    private string _statusMessage = string.Empty;

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    private string _favoriteStatusMessage = string.Empty;

    public string FavoriteStatusMessage
    {
        get => _favoriteStatusMessage;
        set => SetProperty(ref _favoriteStatusMessage, value);
    }

    private bool _isSavingFavorite;

    public bool IsSavingFavorite
    {
        get => _isSavingFavorite;
        private set
        {
            if (SetProperty(ref _isSavingFavorite, value))
            {
                SaveFavoriteCommand.ChangeCanExecute();
            }
        }
    }

    public Command SaveFavoriteCommand { get; }

    public ModelsViewModel(
        IApiService api,
        FavoriteMakeService favoriteService)
    {
        _api = api;
        _favoriteService = favoriteService;

        SaveFavoriteCommand = new Command(
            async () => await SaveFavoriteAsync(),
            () => !IsSavingFavorite && MakeId > 0);
    }

    public void ApplyQueryAttributes(
        IDictionary<string, object> query)
    {
        FavoriteStatusMessage = string.Empty;

        if (query.TryGetValue("makeId", out var makeIdValue))
        {
            MakeId = Convert.ToInt32(makeIdValue);
        }

        if (query.TryGetValue("makeName", out var makeNameValue))
        {
            MakeName =
                makeNameValue.ToString() ?? string.Empty;
        }

        if (query.TryGetValue(
            "vehicleTypeName",
            out var vehicleTypeValue))
        {
            VehicleTypeName =
                vehicleTypeValue.ToString() ?? string.Empty;
        }

        if (MakeId > 0)
        {
            _ = LoadModelsAsync();
        }
    }

    private async Task SaveFavoriteAsync()
    {
        if (IsSavingFavorite || MakeId <= 0)
            return;

        try
        {
            IsSavingFavorite = true;
            FavoriteStatusMessage = "Guardando marca...";

            var favorite = new FavoriteMake
            {
                MakeId = MakeId,
                MakeName = MakeName,
                VehicleTypeName = VehicleTypeName,
                AddedAt = DateTime.Now
            };

            var saved = await _favoriteService.AddAsync(favorite);

            FavoriteStatusMessage = saved
                ? "Marca guardada correctamente."
                : "Esta marca ya esta en favoritos.";
        }
        catch (ArgumentException ex)
        {
            FavoriteStatusMessage = ex.Message;
        }
        catch (Exception)
        {
            FavoriteStatusMessage =
                "No se pudo guardar la marca en favoritos.";
        }
        finally
        {
            IsSavingFavorite = false;
        }
    }

    private async Task LoadModelsAsync()
    {
        StatusMessage = string.Empty;
        Models.Clear();

        IsBusy = true;

        try
        {
            var models =
                await _api.GetModelsForMakeAsync(MakeId);

            foreach (var model in models.OrderBy(m => m.ModelName))
            {
                Models.Add(model);
            }

            StatusMessage = Models.Count > 0
                ? $"Se cargaron {Models.Count} modelos."
                : "No se encontraron modelos para esta marca.";
        }
        catch (Exception ex)
        {
            StatusMessage = ApiErrorMessages.GetMessage(
                ex,
                "Ocurrio un error inesperado al cargar los modelos.");
        }
        finally
        {
            IsBusy = false;
        }
    }
}