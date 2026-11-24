using Moq;
using Parcial1.Core.Models;
using Parcial1.Core.Repositories;
using Parcial1.Core.Services;
using Xunit;

namespace Parcial1.Tests.UnitTests;

public class FavoriteMakeServiceTests
{
    [Fact]
    public async Task GetAllAsync_RetornaFavoritos()
    {
        // Arrange
        var favorites = new List<FavoriteMake>
        {
            new FavoriteMake
            {
                MakeId = 1,
                MakeName = "Toyota"
            },
            new FavoriteMake
            {
                MakeId = 2,
                MakeName = "Ford"
            }
        };

        var repository = new Mock<IFavoriteMakeRepository>();

        repository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(favorites);

        var service = new FavoriteMakeService(repository.Object);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count);

        repository.Verify(
            r => r.GetAllAsync(),
            Times.Once);
    }

    [Fact]
    public async Task AddAsync_MarcaNueva_GuardaFavorito()
    {
        // Arrange
        var favorite = new FavoriteMake
        {
            MakeId = 1,
            MakeName = "Toyota"
        };

        var repository = new Mock<IFavoriteMakeRepository>();

        repository
            .Setup(r => r.GetByIdAsync(favorite.MakeId))
            .ReturnsAsync((FavoriteMake?)null);

        repository
            .Setup(r => r.InsertAsync(favorite))
            .ReturnsAsync(1);

        var service = new FavoriteMakeService(repository.Object);

        // Act
        var result = await service.AddAsync(favorite);

        // Assert
        Assert.True(result);

        repository.Verify(
            r => r.InsertAsync(favorite),
            Times.Once);
    }

    [Fact]
    public async Task AddAsync_MarcaDuplicada_NoGuardaFavorito()
    {
        var favorite = new FavoriteMake
        {
            MakeId = 1,
            MakeName = "Toyota"
        };

        var repository = new Mock<IFavoriteMakeRepository>();

        repository
            .Setup(r => r.GetByIdAsync(favorite.MakeId))
            .ReturnsAsync(favorite);

        var service = new FavoriteMakeService(repository.Object);

        var result = await service.AddAsync(favorite);

        Assert.False(result);

        repository.Verify(
            r => r.InsertAsync(It.IsAny<FavoriteMake>()),
            Times.Never);
    }

    [Fact]
    public async Task AddAsync_MarcaInvalida_LanzaExcepcion()
    {
        var favorite = new FavoriteMake
        {
            MakeId = 0,
            MakeName = ""
        };

        var repository = new Mock<IFavoriteMakeRepository>();

        var service = new FavoriteMakeService(repository.Object);

        await Assert.ThrowsAsync<ArgumentException>(
            () => service.AddAsync(favorite));

        repository.Verify(
            r => r.GetByIdAsync(It.IsAny<int>()),
            Times.Never);

        repository.Verify(
            r => r.InsertAsync(It.IsAny<FavoriteMake>()),
            Times.Never);
    }

    [Fact]
    public async Task AddAsync_InsercionSinCambios_RetornaFalse()
    {
        var favorite = new FavoriteMake
        {
            MakeId = 1,
            MakeName = "Toyota"
        };

        var repository = new Mock<IFavoriteMakeRepository>();

        repository
            .Setup(r => r.GetByIdAsync(favorite.MakeId))
            .ReturnsAsync((FavoriteMake?)null);

        repository
            .Setup(r => r.InsertAsync(favorite))
            .ReturnsAsync(0);

        var service = new FavoriteMakeService(repository.Object);

        var result = await service.AddAsync(favorite);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_FavoritoExistente_EliminaMarca()
    {
        var favorite = new FavoriteMake
        {
            MakeId = 1,
            MakeName = "Toyota"
        };

        var repository = new Mock<IFavoriteMakeRepository>();

        repository
            .Setup(r => r.GetByIdAsync(favorite.MakeId))
            .ReturnsAsync(favorite);

        repository
            .Setup(r => r.DeleteAsync(favorite))
            .ReturnsAsync(1);

        var service = new FavoriteMakeService(repository.Object);

        var result = await service.DeleteAsync(favorite.MakeId);

        Assert.True(result);

        repository.Verify(
            r => r.DeleteAsync(favorite),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_FavoritoInexistente_RetornaFalse()
    {
        var repository = new Mock<IFavoriteMakeRepository>();

        repository
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync((FavoriteMake?)null);

        var service = new FavoriteMakeService(repository.Object);

        var result = await service.DeleteAsync(1);

        Assert.False(result);

        repository.Verify(
            r => r.DeleteAsync(It.IsAny<FavoriteMake>()),
            Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_IdInvalido_LanzaExcepcion()
    {
        var repository = new Mock<IFavoriteMakeRepository>();

        var service = new FavoriteMakeService(repository.Object);

        await Assert.ThrowsAsync<ArgumentException>(
            () => service.DeleteAsync(0));

        repository.Verify(
            r => r.GetByIdAsync(It.IsAny<int>()),
            Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_EliminacionSinCambios_RetornaFalse()
    {
        var favorite = new FavoriteMake
        {
            MakeId = 1,
            MakeName = "Toyota"
        };

        var repository = new Mock<IFavoriteMakeRepository>();

        repository
            .Setup(r => r.GetByIdAsync(favorite.MakeId))
            .ReturnsAsync(favorite);

        repository
            .Setup(r => r.DeleteAsync(favorite))
            .ReturnsAsync(0);

        var service = new FavoriteMakeService(repository.Object);

        var result = await service.DeleteAsync(favorite.MakeId);

        Assert.False(result);
    }
}