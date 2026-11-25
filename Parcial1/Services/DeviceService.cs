using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;

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

    public async Task<string?> CapturePhotoAsync()
    {
        if (!OperatingSystem.IsAndroid() &&
            !OperatingSystem.IsIOS())
        {
            throw new FeatureNotSupportedException();
        }

        if (!MediaPicker.Default.IsCaptureSupported)
        {
            throw new FeatureNotSupportedException();
        }

        var status =
            await Permissions.CheckStatusAsync<
                Permissions.Camera>();

        if (status != PermissionStatus.Granted)
        {
            status =
                await Permissions.RequestAsync<
                    Permissions.Camera>();
        }

        if (status != PermissionStatus.Granted)
        {
            throw new UnauthorizedAccessException(
                "No se otorgo el permiso de camara.");
        }

        var photo =
            await MediaPicker.Default.CapturePhotoAsync();

        if (photo is null)
        {
            return null;
        }

        var folder = Path.Combine(
            FileSystem.AppDataDirectory,
            "fotos");

        Directory.CreateDirectory(folder);

        var extension = Path.GetExtension(photo.FileName);

        if (string.IsNullOrWhiteSpace(extension))
        {
            extension = ".jpg";
        }

        var fileName = $"{Guid.NewGuid()}{extension}";

        var photoPath = Path.Combine(folder, fileName);

        try
        {
            using var sourceStream =
                await photo.OpenReadAsync();

            using var destinationStream =
                File.Create(photoPath);

            await sourceStream.CopyToAsync(
                destinationStream);

            return photoPath;
        }
        catch
        {
            DeletePhoto(photoPath);
            throw;
        }
    }

    public void DeletePhoto(string? photoPath)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(photoPath) &&
                File.Exists(photoPath))
            {
                File.Delete(photoPath);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
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