using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices.Sensors;
using Parcial1.ViewModels;

namespace Parcial1;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;

    private bool _monitoringShake;
    private DateTime _lastShake = DateTime.MinValue;

    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        StartAccelerometer();
    }

    protected override void OnDisappearing()
    {
        StopAccelerometer();

        base.OnDisappearing();
    }

    private void StartAccelerometer()
    {
        if (!OperatingSystem.IsAndroid() &&
            !OperatingSystem.IsIOS())
        {
            return;
        }

        if (_monitoringShake ||
            !Accelerometer.Default.IsSupported ||
            Accelerometer.Default.IsMonitoring)
        {
            return;
        }

        try
        {
            Accelerometer.Default.ShakeDetected += OnShakeDetected;

            Accelerometer.Default.Start(SensorSpeed.Game);

            _monitoringShake = true;
        }
        catch (Exception ex)
        {
            Accelerometer.Default.ShakeDetected -= OnShakeDetected;

            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }

    private void StopAccelerometer()
    {
        if (!_monitoringShake)
            return;

        _monitoringShake = false;

        Accelerometer.Default.ShakeDetected -= OnShakeDetected;

        try
        {
            if (Accelerometer.Default.IsMonitoring)
            {
                Accelerometer.Default.Stop();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }

    private void OnShakeDetected(object? sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (!_monitoringShake || _viewModel.IsBusy)
                return;

            var now = DateTime.UtcNow;

            if ((now - _lastShake).TotalSeconds < 3)
                return;

            if (_viewModel.LoadCarMakesCommand.CanExecute(null))
            {
                _lastShake = now;

                _viewModel.LoadCarMakesCommand.Execute(null);
            }
        });
    }
}