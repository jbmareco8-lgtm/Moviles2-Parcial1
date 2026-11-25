using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Devices.Sensors;

namespace Parcial1.Services;

public class DeviceService
{
    public async Task<Location?> GetLocationAsync()
    {
        if (!OperatingSystem.IsAndroid() &&
            !OperatingSystem.IsIOS())
        {
            throw new FeatureNotSupportedException();
        }

        var status =
            await Permissions.CheckStatusAsync<
                Permissions.LocationWhenInUse>();

        if (status != PermissionStatus.Granted)
        {
            status =
                await Permissions.RequestAsync<
                    Permissions.LocationWhenInUse>();
        }

        if (status != PermissionStatus.Granted)
        {
            throw new UnauthorizedAccessException(
                "No se otorgo el permiso de ubicacion.");
        }

        var request = new GeolocationRequest(
            GeolocationAccuracy.Medium,
            TimeSpan.FromSeconds(10));

        return await Geolocation.Default.GetLocationAsync(
            request);
    }

    public void VibrateConfirmation()
    {
        try
        {
            if (Vibration.Default.IsSupported)
            {
                Vibration.Default.Vibrate(
                    TimeSpan.FromMilliseconds(200));
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }
}