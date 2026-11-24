using Parcial1.Core.Models;

namespace Parcial1.Core.Repositories;

public interface IFavoriteMakeRepository
{
    Task<List<FavoriteMake>> GetAllAsync();

    Task<FavoriteMake?> GetByIdAsync(int makeId);

    Task<int> InsertAsync(FavoriteMake favorite);

    Task<int> UpdateAsync(FavoriteMake favorite);

    Task<int> DeleteAsync(FavoriteMake favorite);
}