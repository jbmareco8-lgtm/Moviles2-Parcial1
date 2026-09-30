using System.Text.Json;
using Parcial1.Models;

namespace Parcial1.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<CarMake>> GetCarMakesAsync(
        CancellationToken ct = default)
    {
        const string url = "GetMakesForVehicleType/car?format=json";

        var response = await _httpClient.GetAsync(url, ct);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(ct);

        var data = JsonSerializer.Deserialize<CarMakeResponse>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return data?.Results ?? new List<CarMake>();
    }

    public async Task<IReadOnlyList<CarModel>> GetModelsForMakeAsync(
    int makeId,
    CancellationToken ct = default)
    {
        var url = $"GetModelsForMakeId/{makeId}?format=json";

        var response = await _httpClient.GetAsync(url, ct);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(ct);

        var data = JsonSerializer.Deserialize<CarModelResponse>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return data?.Results ?? new List<CarModel>();
    }
}