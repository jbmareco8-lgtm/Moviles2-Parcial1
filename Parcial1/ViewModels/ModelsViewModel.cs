using System.Collections.ObjectModel;
using System.Text.Json;
using Parcial1.Models;
using Parcial1.Services;

namespace Parcial1.ViewModels;

public class ModelsViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IApiService _api;

    public ObservableCollection<CarModel> Models { get; } = new();

    private int _makeId;

    public int MakeId
    {
        get => _makeId;
        set => SetProperty(ref _makeId, value);
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

    public ModelsViewModel(IApiService api)
    {
        _api = api;
    }

    public void ApplyQueryAttributes(
        IDictionary<string, object> query)
    {
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

            StatusMessage =
                $"Se cargaron {Models.Count} modelos.";
        }
        catch (HttpRequestException ex)
            when (ex.StatusCode is null)
        {
            StatusMessage =
                "No se pudo conectar con el servidor. Verificá tu conexión a Internet.";
        }
        catch (HttpRequestException ex)
        {
            StatusMessage =
                $"Error HTTP {(int?)ex.StatusCode}: {ex.StatusCode}.";
        }
        catch (TaskCanceledException)
        {
            StatusMessage =
                "La solicitud tardó demasiado tiempo. Intentá nuevamente.";
        }
        catch (JsonException)
        {
            StatusMessage =
                "No se pudo interpretar la respuesta recibida de la API.";
        }
        catch (Exception)
        {
            StatusMessage =
                "Ocurrió un error inesperado al cargar los modelos.";
        }
        finally
        {
            IsBusy = false;
        }
    }
}