using System.Collections.ObjectModel;
using System.Windows.Input;
using Parcial1.Models;
using Parcial1.Services;
using Parcial1.Core.Services;

namespace Parcial1.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly IApiService _apiService;

    private readonly List<CarMake> _allCarMakes = new();

    private bool _isBusy;
    private string _statusMessage = string.Empty;
    private string _searchText = string.Empty;

    public ObservableCollection<CarMake> CarMakes { get; } = new();

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

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
            {
                FilterCarMakes();
            }
        }
    }

    public ICommand LoadCarMakesCommand { get; }
    public ICommand SelectMakeCommand { get; }

    public MainViewModel(IApiService apiService)
    {
        _apiService = apiService;

        LoadCarMakesCommand = new Command(
            async () => await LoadCarMakesAsync(),
            () => !IsBusy);

        SelectMakeCommand = new Command<CarMake>(
            async make => await SelectMakeAsync(make));
    }

    private async Task LoadCarMakesAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            StatusMessage = "Cargando marcas...";

            var makes = await _apiService.GetCarMakesAsync();

            _allCarMakes.Clear();

            foreach (var make in makes.OrderBy(m => m.MakeName))
            {
                _allCarMakes.Add(make);
            }

            FilterCarMakes();

            StatusMessage = CarMakes.Count > 0
                ? $"Se cargaron {CarMakes.Count} marcas."
                : "No se encontraron marcas para mostrar.";
        }
        catch (Exception ex)
        {
            StatusMessage = ApiErrorMessages.GetMessage(
                ex,
                "Ocurrio un error inesperado al cargar las marcas.");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void FilterCarMakes()
    {
        CarMakes.Clear();

        IEnumerable<CarMake> filteredMakes = _allCarMakes;

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            filteredMakes = _allCarMakes.Where(make =>
                make.MakeName.Contains(
                    SearchText.Trim(),
                    StringComparison.OrdinalIgnoreCase));
        }

        foreach (var make in filteredMakes)
        {
            CarMakes.Add(make);
        }
    }

    private async Task SelectMakeAsync(CarMake? make)
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
            parameters);
    }

    private void UpdateCanExecutes()
    {
        if (LoadCarMakesCommand is Command loadCommand)
        {
            loadCommand.ChangeCanExecute();
        }
    }
}