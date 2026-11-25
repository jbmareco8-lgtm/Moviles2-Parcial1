using Parcial1.Core.Models;
using Parcial1.Core.Repositories;
using Parcial1.Core.Validators;

namespace Parcial1.Core.Services;

public class FavoriteMakeService
{
    private readonly IFavoriteMakeRepository _repository;

    public FavoriteMakeService(
        IFavoriteMakeRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<FavoriteMake>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<bool> AddAsync(FavoriteMake favorite)
    {
        FavoriteMakeValidator.Validate(favorite);

        var existing = await _repository.GetByIdAsync(
            favorite.MakeId);

        if (existing is not null)
        {
            return false;
        }

        var result = await _repository.InsertAsync(favorite);

        return result > 0;
    }

    public async Task<bool> DeleteAsync(int makeId)
    {
        FavoriteMakeValidator.ValidateId(makeId);

        var favorite = await _repository.GetByIdAsync(makeId);

        if (favorite is null)
        {
            return false;
        }

        var result = await _repository.DeleteAsync(favorite);

        return result > 0;
    }

    public async Task<FavoriteMake?> UpdateLocationAsync(
        int makeId,
        double latitude,
        double longitude)
    {
        FavoriteMakeValidator.ValidateId(makeId);

        FavoriteMakeValidator.ValidateLocation(
            latitude, longitude);

        var favorite = await _repository.GetByIdAsync(makeId);

        if (favorite is null)
        {
            return null;
        }

        favorite.Latitude = latitude;
        favorite.Longitude = longitude;

        var result = await _repository.UpdateAsync(favorite);

        return result > 0 ? favorite : null;
    }
}