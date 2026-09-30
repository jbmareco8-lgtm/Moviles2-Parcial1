using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using Parcial1.Models;
using Parcial1.Services;

namespace Parcial1.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly IApiService _api;

    public ObservableCollection<CarMake> CarMakes { get; } = new();

    private bool _isBusy;

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (SetProperty(ref _isBusy, value))
            {
                UpdateCanExecutes();
            }
        }
    }

    private string _statusMessage = string.Empty;

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public ICommand LoadCarMakesCommand { get; }

    public ICommand SelectMakeCommand { get; }

    public MainViewModel(IApiService api)
    {
        _api = api;

        LoadCarMakesCommand = new Command(
            async () => await LoadCarMakesAsync(),
            () => !IsBusy
        );

        SelectMakeCommand = new Command<CarMake>(
            async make => await GoToModelsAsync(make)
        );
    }

    private void UpdateCanExecutes()
    {
        (LoadCarMakesCommand as Command)?.ChangeCanExecute();
    }

    private async Task LoadCarMakesAsync()
    {
        StatusMessage = string.Empty;
        CarMakes.Clear();

        IsBusy = true;

        try
        {
            var makes = await _api.GetCarMakesAsync();

            foreach (var make in makes)
            {
                CarMakes.Add(make);
            }

            StatusMessage =
                $"Se cargaron {CarMakes.Count} marcas.";
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
        catch (JsonException)
        {
            StatusMessage =
                "No se pudo interpretar la respuesta recibida de la API.";
        }
        catch (Exception)
        {
            StatusMessage =
                "Ocurrió un error inesperado al cargar las marcas.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task GoToModelsAsync(CarMake? make)
    {
        if (make is null)
            return;

        var parameters = new ShellNavigationQueryParameters
        {
            { "makeId", make.MakeId },
            { "makeName", make.MakeName },
            { "vehicleTypeName", make.VehicleTypeName }
        };

        await Shell.Current.GoToAsync(
            nameof(ModelsPage),
            parameters
        );
    }
}