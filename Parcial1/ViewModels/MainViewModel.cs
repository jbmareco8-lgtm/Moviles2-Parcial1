using System.Collections.ObjectModel;
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

    public MainViewModel(IApiService api)
    {
        _api = api;

        LoadCarMakesCommand = new Command(
            async () => await LoadCarMakesAsync(),
            () => !IsBusy
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

            StatusMessage = $"Se cargaron {CarMakes.Count} marcas.";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }
}