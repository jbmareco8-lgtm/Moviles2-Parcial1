using SQLite;
using Parcial1.Core.Models;
using Parcial1.Core.Repositories;
using Parcial1.Core.Services;
using Xunit;

namespace Parcial1.Tests.IntegrationTests;

public class FavoriteMakeRepositoryTests : IAsyncLifetime
{
    private readonly DatabaseService _databaseService;
    private readonly FavoriteMakeRepository _repository;
    private SQLiteAsyncConnection? _connection;

    public FavoriteMakeRepositoryTests()
    {
        _databaseService = new DatabaseService(":memory:");

        _repository = new FavoriteMakeRepository(
            _databaseService);
    }

    public async Task InitializeAsync()
    {
        _connection =
            await _databaseService.GetConnectionAsync();
    }

    public async Task DisposeAsync()
    {
        if (_connection is not null)
        {
            await _connection.CloseAsync();
        }
    }

    [Fact]
    public async Task GetAllAsync_BaseVacia_RetornaListaVacia()
    {
        var result = await _repository.GetAllAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task InsertAsync_GuardaMarcaCorrectamente()
    {
        var favorite = new FavoriteMake
        {
            MakeId = 1,
            MakeName = "Toyota",
            VehicleTypeName = "Passenger Car"
        };

        var inserted = await _repository.InsertAsync(favorite);

        var result = await _repository.GetByIdAsync(1);

        Assert.Equal(1, inserted);
        Assert.NotNull(result);
        Assert.Equal(1, result.MakeId);
        Assert.Equal("Toyota", result.MakeName);
        Assert.Equal("Passenger Car", result.VehicleTypeName);
    }

    [Fact]
    public async Task GetAllAsync_RetornaMarcasOrdenadas()
    {
        await _repository.InsertAsync(new FavoriteMake
        {
            MakeId = 1,
            MakeName = "Toyota"
        });

        await _repository.InsertAsync(new FavoriteMake
        {
            MakeId = 2,
            MakeName = "Ford"
        });

        var result = await _repository.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal("Ford", result[0].MakeName);
        Assert.Equal("Toyota", result[1].MakeName);
    }

    [Fact]
    public async Task UpdateAsync_ModificaMarcaGuardada()
    {
        var favorite = new FavoriteMake
        {
            MakeId = 1,
            MakeName = "Toyota"
        };

        await _repository.InsertAsync(favorite);

        favorite.VehicleTypeName = "Passenger Car";

        var updated = await _repository.UpdateAsync(favorite);

        var result = await _repository.GetByIdAsync(1);

        Assert.Equal(1, updated);
        Assert.NotNull(result);
        Assert.Equal("Passenger Car", result.VehicleTypeName);
    }

    [Fact]
    public async Task DeleteAsync_EliminaMarcaGuardada()
    {
        var favorite = new FavoriteMake
        {
            MakeId = 1,
            MakeName = "Toyota"
        };

        await _repository.InsertAsync(favorite);

        var deleted = await _repository.DeleteAsync(favorite);

        var result = await _repository.GetByIdAsync(1);

        Assert.Equal(1, deleted);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_MarcaInexistente_RetornaNull()
    {
        var result = await _repository.GetByIdAsync(100);

        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ServicioYRepositorio_EvitanDuplicados()
    {
        var service = new FavoriteMakeService(_repository);

        var favorite = new FavoriteMake
        {
            MakeId = 1,
            MakeName = "Toyota"
        };

        var firstResult = await service.AddAsync(favorite);
        var secondResult = await service.AddAsync(favorite);

        var favorites = await service.GetAllAsync();

        Assert.True(firstResult);
        Assert.False(secondResult);
        Assert.Single(favorites);
    }

    [Fact]
    public async Task UpdateAsync_GuardaUbicacionYFoto()
    {
        // Arrange
        var service = new FavoriteMakeService(_repository);

        var favorite = new FavoriteMake
        {
            MakeId = 1,
            MakeName = "Toyota",
            VehicleTypeName = "Passenger Car"
        };

        Assert.True(await service.AddAsync(favorite));

        // Act
        var location = await service.UpdateLocationAsync(
            1, -34.42, -58.57);

        var photo = await service.UpdatePhotoAsync(
            1, "fotos/toyota.jpg");

        var result = await _repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(location);
        Assert.NotNull(photo);
        Assert.NotNull(result);

        Assert.Equal<double?>(-34.42, result.Latitude);
        Assert.Equal<double?>(-58.57, result.Longitude);
        Assert.Equal("fotos/toyota.jpg", result.PhotoPath);
        Assert.Equal("Toyota", result.MakeName);
        Assert.Equal("Passenger Car", result.VehicleTypeName);
        Assert.True(result.HasPhoto);
    }

    [Fact]
    public async Task UpdateAsync_MarcaInexistente_NoGuardaDatos()
    {
        var service = new FavoriteMakeService(_repository);

        var location = await service.UpdateLocationAsync(
            1, -34.42, -58.57);

        var photo = await service.UpdatePhotoAsync(
            1, "fotos/toyota.jpg");

        var favorites = await _repository.GetAllAsync();

        Assert.Null(location);
        Assert.Null(photo);
        Assert.Empty(favorites);
    }
}