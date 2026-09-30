using System.Collections.ObjectModel;
using Parcial1.Models;
using Parcial1.Services;

namespace Parcial1.ViewModels;

public class ModelsViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IApiService _api;

    public ObservableCollection<CarModel> Models { get; } = new();

    private string _makeName = string.Empty;

    public string MakeName
    {
        get => _makeName;
        set => SetProperty(ref _makeName, value);
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

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("makeId", out var makeIdValue) &&
            query.TryGetValue("makeName", out var makeNameValue))
        {
            int makeId = Convert.ToInt32(makeIdValue);
            string makeName =
                makeNameValue.ToString() ?? string.Empty;

            _ = LoadModelsAsync(makeId, makeName);
        }
    }

    private async Task LoadModelsAsync(
        int makeId,
        string makeName)
    {
        MakeName = makeName;
        StatusMessage = string.Empty;
        Models.Clear();

        IsBusy = true;

        try
        {
            var models =
                await _api.GetModelsForMakeAsync(makeId);

            foreach (var model in models)
            {
                Models.Add(model);
            }

            StatusMessage =
                $"Se cargaron {Models.Count} modelos.";
        }
        catch (HttpRequestException ex) when (ex.StatusCode is null)
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