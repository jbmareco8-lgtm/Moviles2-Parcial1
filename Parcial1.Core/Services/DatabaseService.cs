using SQLite;
using Parcial1.Core.Models;

namespace Parcial1.Core.Services;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _database;
    private readonly Task _initialization;

    public DatabaseService(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);

        _initialization =
            _database.CreateTableAsync<FavoriteMake>();
    }

    public async Task<SQLiteAsyncConnection> GetConnectionAsync()
    {
        await _initialization;

        return _database;
    }
}