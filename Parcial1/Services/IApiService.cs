using Parcial1.Models;

namespace Parcial1.Services;

public interface IApiService
{
    Task<IReadOnlyList<CarMake>> GetCarMakesAsync(
        CancellationToken ct = default
    );

    Task<IReadOnlyList<CarModel>> GetModelsForMakeAsync(
        int makeId,
        CancellationToken ct = default
    );
}