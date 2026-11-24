using Parcial1.Core.Models;
using Parcial1.Core.Services;

namespace Parcial1.Core.Repositories;

public class FavoriteMakeRepository : IFavoriteMakeRepository
{
    private readonly DatabaseService _databaseService;

    public FavoriteMakeRepository(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<List<FavoriteMake>> GetAllAsync()
    {
        var database = await _databaseService.GetConnectionAsync();

        return await database.Table<FavoriteMake>()
            .OrderBy(f => f.MakeName)
            .ToListAsync();
    }

    public async Task<FavoriteMake?> GetByIdAsync(int makeId)
    {
        var database = await _databaseService.GetConnectionAsync();

        return await database.FindAsync<FavoriteMake>(makeId);
    }

    public async Task<int> InsertAsync(FavoriteMake favorite)
    {
        var database = await _databaseService.GetConnectionAsync();

        return await database.InsertAsync(favorite);
    }

    public async Task<int> UpdateAsync(FavoriteMake favorite)
    {
        var database = await _databaseService.GetConnectionAsync();

        return await database.UpdateAsync(favorite);
    }

    public async Task<int> DeleteAsync(FavoriteMake favorite)
    {
        var database = await _databaseService.GetConnectionAsync();

        return await database.DeleteAsync(favorite);
    }
}